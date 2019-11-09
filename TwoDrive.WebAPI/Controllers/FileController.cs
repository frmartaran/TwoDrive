using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.DataAccess.Interface;
using TwoDrive.DataAccess.Interface.LogicInput;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Filters;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Models;
using TwoDrive.WebApi.Resource;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private ICurrent inSession;

        private IFolderLogic folderLogic;

        private IFileLogic fileLogic;

        private ILogic<Writer> writerLogic;

        private IModificationLogic modificationLogic;

        private IFolderValidator elementValidator;

        private IRepository<Element> elementRepository;

        public FileController(IFileLogic logicFile, IFolderLogic logicFolder,
            ILogic<Writer> logicWriter, ICurrent session, IModificationLogic logic,
            IFolderValidator validator, IRepository<Element> repository)
        {
            inSession = session;
            folderLogic = logicFolder;
            fileLogic = logicFile;
            writerLogic = logicWriter;
            modificationLogic = logic;
            elementValidator = validator;
            elementRepository = repository;
        }

        [HttpPost("{id}")]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult Create(int id, [FromBody] TxtModel model)
        {
            try
            {
                var loggedWriter = inSession.GetCurrentUser(HttpContext);
                var parentFolder = folderLogic.Get(id);
                if (loggedWriter == null)
                    return BadRequest(ApiResource.MustLogIn);
                if (parentFolder == null)
                    return NotFound(ApiResource.ParentFolderNotFound);
                if (loggedWriter != parentFolder.Owner)
                    return BadRequest(string.Format(ApiResource.NotOwnerOf, parentFolder.Name));


                var file = model.ToDomain() as File;
                file.Owner = loggedWriter;
                file.ParentFolder = parentFolder;
                file.CreationDate = DateTime.Now;
                file.DateModified = DateTime.Now;
                fileLogic.Create(file);
                loggedWriter.AddCreatorClaimsTo(file);
                writerLogic.Update(loggedWriter);

                CreateModification(file, ModificationType.Added);
                folderLogic.CreateModificationsForTree(file, ModificationType.Changed);
                return Ok(new TxtModel().FromDomain(file));
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}")]
        [ClaimFilter(ClaimType.Delete)]
        public IActionResult Delete(int id)
        {
            try
            {
                var file = fileLogic.Get(id);
                fileLogic.Delete(id);
                CreateModification(file, ModificationType.Deleted);
                folderLogic.CreateModificationsForTree(file, ModificationType.Deleted);

                return Ok(string.Format(ApiResource.Delete_FileController, file.Name));
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        [ClaimFilter(ClaimType.Read)]
        public IActionResult Get(int id)
        {
            var file = fileLogic.Get(id);
            if (file == null)
                return NotFound(ApiResource.FileNotFound);

            var model = new TxtModel().FromDomain(file);
            return Ok(model);
        }

        [HttpGet("Content/{id}")]
        [ClaimFilter(ClaimType.Read)]
        public IActionResult DisplayContent(int id)
        {
            try
            {
                var file = fileLogic.Get(id);
                if (file is HTMLFile htmlFile)
                {
                    if (!htmlFile.ShouldRender)
                    {
                        var encodedContent = HttpUtility.HtmlEncode(file.Content);
                        return Ok(encodedContent);
                    }
                }
                return Ok(file.Content);
            }
            catch (NullReferenceException)
            {
                return BadRequest(ApiResource.FileNotFound);
            }
            
            
        }

        [HttpGet]
        public IActionResult GetAll(string name = "", bool isOrderDescending = false, bool isOrderByName = false, 
            bool isOrderByCreationDate = false, bool IsOrderByModificationDate = false)
        {
            var writer = inSession.GetCurrentUser(HttpContext);
            if (writer == null)
                return NotFound(ApiResource.MustLogIn);

            var fileFilter = new FileFilter
            {
                Name = name,
                IsOrderDescending = isOrderDescending,
                IsOrderByName = isOrderByName,
                IsOrderByCreationDate = isOrderByCreationDate,
                IsOrderByModificationDate = IsOrderByModificationDate,
                Id = writer.Role == Role.Writer
                    ? writer.Id
                    : (int?)null
            };

            var files = fileLogic.GetAll(fileFilter);
            var writerfiles = files
                .Select(f => new TxtModel().FromDomain(f))
                .ToList();

            if (writerfiles.Count == 0)
                return NotFound(ApiResource.FilesNotFound);

            return Ok(writerfiles);
        }

        [HttpPut("{id}")]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult Update(int id, [FromBody] TxtModel model)
        {
            try
            {
                var file = fileLogic.Get(id);
                file = model.ToDomain(file as TxtFile);
                fileLogic.Update(file);
                CreateModification(file, ModificationType.Changed);
                folderLogic.CreateModificationsForTree(file, ModificationType.Changed);

                return Ok(ApiResource.FileUpdated);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("{id}/{friendId}")]
        [ClaimFilter(ClaimType.Share)]
        public IActionResult Share(int id, int friendId)
        {
            try
            {
                var writer = inSession.GetCurrentUser(HttpContext);
                if (writer == null)
                    return NotFound(ApiResource.MustLogIn);

                var friend = writerLogic.Get(friendId);
                if (friend == null)
                    return NotFound(ApiResource.FriendNotFound);

                var file = fileLogic.Get(id);
                if (file == null)
                    return NotFound(ApiResource.FileNotFound);
                if (!writer.IsFriendsWith(friend))
                    return BadRequest(string.Format(ApiResource.NotFriends, friend.UserName));

                writer.AllowFriendTo(friend, file, ClaimType.Read);
                writerLogic.Update(friend);
                return Ok(string.Format(ApiResource.Shared, file.Name, friend.UserName));
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}/{friendId}")]
        [ClaimFilter(ClaimType.Share)]
        public IActionResult StopShare(int id, int friendId)
        {
            try
            {
                var writer = inSession.GetCurrentUser(HttpContext);
                if (writer == null)
                    return NotFound(ApiResource.MustLogIn);

                var friend = writerLogic.Get(friendId);
                if (friend == null)
                    return NotFound(ApiResource.FriendNotFound);

                var file = fileLogic.Get(id);
                if (file == null)
                    return NotFound(ApiResource.FileNotFound);
                if (!writer.IsFriendsWith(friend))
                    return BadRequest(string.Format(ApiResource.MustBeFriends, friend.UserName));

                writer.RevokeFriendFrom(friend, file, ClaimType.Read);
                writerLogic.Update(friend);
                return Ok(string.Format(ApiResource.StopSharing, file.Name, friend.UserName));
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("{id}/{folderId}")]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult Move(int id, int folderId)
        {
            var writer = inSession.GetCurrentUser(HttpContext);
            if (writer == null)
                return NotFound(ApiResource.MustLogIn);

            var file = fileLogic.Get(id);
            if (file == null)
                return NotFound(ApiResource.FileNotFound);

            var folder = folderLogic.Get(folderId);
            if (folder == null)
                return NotFound(ApiResource.FolderNotFound);

            if (!writer.IsOwnerOfOriginAndDestination(file, folder))
                return BadRequest(ApiResource.MustOwn_FolderController);

            var dependencies = new MoveElementDependencies
            {
                ElementRepository = elementRepository,
                ElementValidator = elementValidator
            };
            folderLogic.MoveElement(file, folder, dependencies);
            CreateModification(file, ModificationType.Changed);
            CreateModification(folder, ModificationType.Changed);
            folderLogic.CreateModificationsForTree(folder, ModificationType.Changed);
            folderLogic.CreateModificationsForTree(file, ModificationType.Changed);
            return Ok(string.Format(ApiResource.Moved_FileController, file.Name, folder.Name));
        }

        private void CreateModification(Element file, ModificationType action)
        {
            var modification = new Modification
            {
                ElementModified = file,
                type = action,
                Date = file.CreationDate
            };
            modificationLogic.Create(modification);
        }
    }
}