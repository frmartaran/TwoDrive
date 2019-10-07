using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Filters;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private ICurrent inSession;

        private IFolderLogic folderLogic;

        private ILogic<File> fileLogic;

        private ILogic<Writer> writerLogic;

        private IModificationLogic modificationLogic;

        public FileController(ILogic<File> logicFile, IFolderLogic logicFolder,
            ILogic<Writer> logicWriter, ICurrent session, IModificationLogic logic)
        {
            inSession = session;
            folderLogic = logicFolder;
            fileLogic = logicFile;
            writerLogic = logicWriter;
            modificationLogic = logic;
        }

        [HttpPost("{id}")]
        public IActionResult Create(int folderId, [FromBody] FileModel model)
        {
            try
            {
                var loggedWriter = inSession.GetCurrentUser(HttpContext);
                var parentFolder = folderLogic.Get(folderId);
                if (loggedWriter == null)
                    return BadRequest("You must log in first");
                if (parentFolder == null)
                    return NotFound("Parent folder doesn't exist");
                if (loggedWriter != parentFolder.Owner)
                    return BadRequest("You are not owner of this folder");


                var file = model.ToDomain();
                file.Owner = loggedWriter;
                file.ParentFolder = parentFolder;
                file.CreationDate = DateTime.Now;
                file.DateModified = DateTime.Now;
                fileLogic.Create(file);
                loggedWriter.AddCreatorClaimsTo(file);
                writerLogic.Update(loggedWriter);

                CreateModification(file, ModificationType.Added);
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
                return Ok($"{file.Name} has been deleted");
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
                return NotFound("File not found");

            var model = new TxtModel().FromDomain(file);
            return Ok(model);
        }

        [HttpGet]
        [Route("api/[controller]/Admin/{id}")]
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult GetAll(int writerId)
        {
            var files = fileLogic.GetAll();
            var writerfiles = files
                .Where(f => f.OwnerId == writerId)
                .Select(f => new TxtModel().FromDomain(f))
                .ToList();

            if (writerfiles.Count == 0)
                return NotFound("No files found");

            return Ok(writerfiles);

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return null;
        }

        private void CreateModification(File file, ModificationType action)
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