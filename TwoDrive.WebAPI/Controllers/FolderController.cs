using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Filters;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Models;
using TwoDrive.WebApi.Resource;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class FolderController : ControllerBase
    {
        private IFolderLogic FolderLogic { get; set; }

        private ICurrent Session { get; set; }

        private IRepository<Element> ElementRepository { get; set; }

        private IFolderValidator Validator { get; set; }

        private ILogic<Writer> WriterLogic { get; set; }

        private IModificationLogic ModificationLogic { get; set; }

        public FolderController(IFolderLogic folderLogic, ICurrent session,
            IRepository<Element> elementRepository, IFolderValidator validator,
            ILogic<Writer> writerLogic, IModificationLogic modificationLogic) : base()
        {
            FolderLogic = folderLogic;
            Session = session;
            ElementRepository = elementRepository;
            Validator = validator;
            WriterLogic = writerLogic;
            ModificationLogic = modificationLogic;
        }

        [HttpDelete("{id}")]
        [ClaimFilter(ClaimType.Delete)]
        public IActionResult Delete(int id)
        {
            try
            {
                var folder = FolderLogic.Get(id);
                FolderLogic.Delete(id);
                return Ok(string.Format(ApiResource.Deleted_FolderController, folder.Name));
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("{folderToMoveId}/{folderDestinationId}")]
        public IActionResult MoveFolder(int folderToMoveId, int folderDestinationId)
        {
            try
            {
                var writer = Session.GetCurrentUser(HttpContext);
                var folderToMove = FolderLogic.Get(folderToMoveId);
                var folderDestination = FolderLogic.Get(folderDestinationId);
                if (writer.IsOwnerOfOriginAndDestination(folderToMove, folderDestination))
                {
                    var moveElementDependencies = new MoveElementDependencies
                    {
                        ElementRepository = ElementRepository,
                        ElementValidator = Validator
                    };
                    FolderLogic.MoveElement(folderToMove, folderDestination, moveElementDependencies);
                }
                else
                {
                    return BadRequest(ApiResource.MustOwn_FolderController);
                }
                FolderLogic.CreateModificationsForTree(folderToMove, ModificationType.Changed);
                FolderLogic.CreateModificationsForTree(folderDestination, ModificationType.Changed);
                return Ok(string.Format(ApiResource.Moved_FolderController, folderToMove.Name, folderDestination.Name));
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        
        private void CreateModification(Element file, ModificationType action)
        {
            var modification = new Modification
            {
                ElementModified = file,
                type = action,
                Date = file.CreationDate
            };
            ModificationLogic.Create(modification);
        }

        [HttpPut("{id}")]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult Update(int id, [FromBody] FolderModel model)
        {
            try
            {
                var folder = FolderLogic.Get(id);
                folder = model.ToDomain(folder);
                FolderLogic.Update(folder);
                var updatedFolder = FolderLogic.Get(id);
                FolderLogic.CreateModificationsForTree(updatedFolder, ModificationType.Changed);
                var toModel = new FolderModel();
                return Ok(toModel.FromDomain(updatedFolder));
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        [ClaimFilter(ClaimType.Read)]
        public IActionResult Get(int id)
        {
            var folder = FolderLogic.Get(id);
            if (folder == null)
            {
                return NotFound(ApiResource.FolderNotFound);
            }
            var folderModel = new FolderModel();
            return Ok(folderModel.FromDomain(folder));
        }

        [HttpPost("{id}")]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult Create(int id, [FromBody] FolderModel model)
        {
            try
            {
                var loggedWriter = Session.GetCurrentUser(HttpContext);
                var parentFolder = FolderLogic.Get(id);

                if (loggedWriter == null)
                    return NotFound(ApiResource.MustLogIn);
                if (parentFolder == null)
                    return NotFound(ApiResource.ParentFolderNotFound);
                if (loggedWriter != parentFolder.Owner)
                    return BadRequest(string.Format(ApiResource.NotOwnerOf, parentFolder.Name));

                var folder = model.ToDomain();
                folder.Owner = loggedWriter;
                folder.ParentFolder = parentFolder;
                folder.CreationDate = DateTime.Now;
                folder.DateModified = DateTime.Now;
                FolderLogic.Create(folder);
                loggedWriter.AddCreatorClaimsTo(folder);
                WriterLogic.Update(loggedWriter);
                CreateModification(folder, ModificationType.Added);
                FolderLogic.CreateModificationsForTree(parentFolder, ModificationType.Changed);
                return Ok(new FolderModel().FromDomain(folder));
            }
            catch (LogicException exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpGet("Tree/{id}")]
        [ClaimFilter(ClaimType.Read)]
        public IActionResult ShowTree(int id)
        {
            var folder = FolderLogic.Get(id);
            if (folder == null)
                return NotFound(ApiResource.FolderNotFound);

            var tree = FolderLogic.ShowTree(folder);
            return Ok(tree);
        }

        [HttpPut("{id}/{friendId}")]
        [ClaimFilter(ClaimType.Share)]
        public IActionResult Share(int id, int friendId)
        {
            try
            {
                var writer = Session.GetCurrentUser(HttpContext);
                if (writer == null)
                    return NotFound(ApiResource.MustLogIn);

                var friend = WriterLogic.Get(friendId);
                if (friend == null)
                    return NotFound(ApiResource.FriendNotFound);

                var folder = FolderLogic.Get(id);
                if (folder == null)
                    return NotFound(ApiResource.FolderNotFound);

                writer.AllowFriendTo(friend, folder, ClaimType.Read);
                WriterLogic.Update(friend);
                return Ok(string.Format(ApiResource.Shared, folder.Name, friend.UserName));
            }
            catch (LogicException exception)
            {
                return BadRequest(exception);
            }
        }

        [HttpDelete("{id}/{friendId}")]
        [ClaimFilter(ClaimType.Share)]
        public IActionResult StopShare(int id, int friendId)
        {
            try
            {
                var writer = Session.GetCurrentUser(HttpContext);
                if (writer == null)
                    return NotFound(ApiResource.MustLogIn);

                var friend = WriterLogic.Get(friendId);
                if (friend == null)
                    return NotFound(ApiResource.FriendNotFound);

                var folder = FolderLogic.Get(id);
                if (folder == null)
                    return NotFound(ApiResource.FolderNotFound);

                if (!writer.IsFriendsWith(friend))
                    return BadRequest(string.Format(ApiResource.MustBeFriends, friend.UserName));

                writer.RevokeFriendFrom(friend, folder, ClaimType.Read);
                WriterLogic.Update(friend);
                return Ok(string.Format(ApiResource.StopSharing, folder.Name, friend.UserName));
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
