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

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
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
                return Ok($"Folder: {folder.Name} has been deleted");
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
                    return BadRequest("You must own both elements");
                }
                return Ok($"Folder with id {folderToMoveId} was moved to destination with id {folderDestinationId}");
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
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
                var updatedWriter = FolderLogic.Get(id);
                var toModel = new FolderModel();
                return Ok(toModel.FromDomain(folder));
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
                return NotFound("Folder not found");
            }
            var folderModel = new FolderModel();
            return Ok(folderModel.FromDomain(folder));
        }

        [HttpPost("{id}")]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult Create(int id, [FromBody] FolderModel model)
        {
            var loggedWriter = Session.GetCurrentUser(HttpContext);
            var parentFolder = FolderLogic.Get(id);

            if (loggedWriter == null)
                return BadRequest("You must log in first");
            if (parentFolder == null)
                return NotFound("Parent folder doesn't exist");
            if (loggedWriter != parentFolder.Owner)
                return BadRequest("You are not owner of this folder");

            var folder = model.ToDomain();
            folder.Owner = loggedWriter;
            folder.ParentFolder = parentFolder;
            folder.CreationDate = DateTime.Now;
            folder.DateModified = DateTime.Now;
            FolderLogic.Create(folder);
            loggedWriter.AddCreatorClaimsTo(folder);
            WriterLogic.Update(loggedWriter);
            var modification = new Modification
            {
                ElementModified = folder,
                type = ModificationType.Added,
                Date = folder.CreationDate
            };
            ModificationLogic.Create(modification);
            return Ok(new FolderModel().FromDomain(folder));
        }

        [HttpGet("Tree/{id}")]
        [ClaimFilter(ClaimType.Read)]
        public IActionResult ShowTree(int id)
        {
            var folder = FolderLogic.Get(id);
            if (folder == null)
                return NotFound("Folder not found");

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
                    return NotFound("You must log in first");

                var friend = WriterLogic.Get(friendId);
                if (friend == null)
                    return NotFound("Friend not found");

                var folder = FolderLogic.Get(id);
                if (folder == null)
                    return NotFound("Folder not found");

                writer.AllowFriendTo(friend, folder, ClaimType.Read);
                WriterLogic.Update(friend);
                return Ok($"{folder.Name} has been shared with {friend.UserName}");
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
                    return NotFound("You must log in first");

                var friend = WriterLogic.Get(friendId);
                if (friend == null)
                    return NotFound("Friend not found");

                var folder = FolderLogic.Get(id);
                if (folder == null)
                    return NotFound("Folder not found");

                if (!writer.IsFriendsWith(friend))
                    return BadRequest($"You must be friends with {friend.UserName}");

                writer.RevokeFriendFrom(friend, folder, ClaimType.Read);
                WriterLogic.Update(friend);
                return Ok($"Stopped sharing {folder.Name} with {friend.UserName}");
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
