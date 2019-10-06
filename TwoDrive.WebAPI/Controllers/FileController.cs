using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
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
        public IActionResult Create(int folderId, [FromBody] TxtModel model)
        {
            var loggedWriter = inSession.GetCurrentUser(HttpContext);
            var parentFolder = folderLogic.Get(folderId);
            var file = model.ToDomain();
            file.Owner = loggedWriter;
            file.ParentFolder = parentFolder;
            file.CreationDate = DateTime.Now;
            file.DateModified = DateTime.Now;
            fileLogic.Create(file);
            loggedWriter.AddCreatorClaimsTo(file);
            writerLogic.Update(loggedWriter);
            var modification = new Modification
            {
                ElementModified = file,
                type = ModificationType.Added,
                Date = file.CreationDate
            };
            modificationLogic.Create(modification);
            return Ok(new TxtModel().FromDomain(file));
        }


    }
}