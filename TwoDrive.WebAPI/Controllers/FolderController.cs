using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Filters;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FolderController : ControllerBase
    {
        private IFolderLogic FolderLogic { get; set; }

        private ISessionLogic Session { get; set; }

        private IRepository<Element> ElementRepository { get; set; }

        private IElementValidator Validator { get; set; }

        public FolderController(IFolderLogic folderLogic, ISessionLogic session, 
            IRepository<Element> elementRepository, IElementValidator validator) : base()
        {
            FolderLogic = folderLogic;
            Session = session;
            ElementRepository = elementRepository;
            Validator = validator;
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

        [HttpPost]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult MoveFolder(int folderToMoveId, int folderDestinationId)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                var writer = Session.GetWriter(token);
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
    }
}
