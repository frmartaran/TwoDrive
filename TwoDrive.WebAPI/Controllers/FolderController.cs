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

        private IElementValidator Validator { get; set; }

        private ILogic<Writer> WriterLogic { get; set; }

        private IModificationLogic ModificationLogic { get; set; }

        public FolderController(IFolderLogic folderLogic, ICurrent session, 
            IRepository<Element> elementRepository, IElementValidator validator,
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
        public IActionResult Delete(int Id)
        {
            try
            {            
                var folder = FolderLogic.Get(Id);
                FolderLogic.Delete(Id);
                return Ok($"Folder: {folder.Name} has been deleted");
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        public IActionResult MoveFolder(int folderToMoveId, int folderDestinationId)
        {
            try
            {
                var writer = Session.GetCurrentUser(HttpContext);
                var folderToMove = FolderLogic.Get(folderToMoveId);
                var folderDestination = FolderLogic.Get(folderDestinationId);
                if (FolderLogicExtension.IsWriterOwnerOfOriginAndDestination(writer, folderToMove, folderDestination))
                {
                    var moveElementDependencies = new MoveElementDependencies
                    {
                        ElementRepository = ElementRepository,
                        ElementValidator = Validator
                    };
                    FolderLogic.MoveElement(folderToMove, folderDestination, moveElementDependencies);
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
        public IActionResult Update(int Id, [FromBody] FolderModel model)
        {
            try
            {
                var folder = FolderLogic.Get(Id);
                var updatedFolder = model.ToDomain();
                folder = updatedFolder;
                FolderLogic.Update(folder);
                var updatedWriter = FolderLogic.Get(Id);
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
        public IActionResult Get(int Id)
        {
            var folder = FolderLogic.Get(Id);
            if (folder == null)
            {
                return NotFound("Folder not found");
            }
            var folderModel = new FolderModel();
            return Ok(folderModel.FromDomain(folder));
        }

        [HttpPost("{id}")]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult Create(int Id, [FromBody] FolderModel model)
        {
            var loggedWriter = Session.GetCurrentUser(HttpContext);
            var parentFolder = FolderLogic.Get(Id);
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
    }
}
