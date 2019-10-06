using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.WebApi.Filters;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FolderController : ControllerBase
    {
        private IFolderLogic FolderLogic { get; set; }

        private ISessionLogic SessionLogic { get; set; }

        public FolderController(IFolderLogic folderLogic, ISessionLogic sessions) : base()
        {
            FolderLogic = folderLogic;
            SessionLogic = sessions;
        }

        [HttpDelete("{id}")]
        [ClaimFilter("{id}", )]
        public IActionResult Delete(int id)
        {
            try
            {
                
                var writer = Logic.Get(id);
                var folder = FolderLogic.GetRootFolder(writer);
                FolderLogic.Delete(folder.Id);
                Logic.Delete(id);
                return Ok($"Writer: {writer.UserName} has been deleted");
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
