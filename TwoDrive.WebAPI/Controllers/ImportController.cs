using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Helpers;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.WebApi.Filters;
using TwoDrive.Importer.Interface;
using TwoDrive.WebApi.Resource;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using Microsoft.AspNetCore.Cors;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    [AuthorizeFilter(Role.Administrator)]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private IImporterLogic ImporterLogic;

        private ILogic<Writer> WriterLogic;

        public ImportController(IImporterLogic importerlogic, ILogic<Writer> writerLogic)
        {
            ImporterLogic = importerlogic;
            WriterLogic = writerLogic;
        }

        [HttpPost("{importType}/{ownerId}")]
        public IActionResult Import([FromBody] ImportingParameters parameters, string importType, int ownerId)
        {
            var owner = WriterLogic.Get(ownerId);
            if (owner == null)
                return BadRequest(ApiResource.WriterNotFound);
            try
            {
                var options = new ImportingOptions
                {
                    ImporterName = importType,
                    Owner = owner,
                    Parameters = parameters
                };
                ImporterLogic.Options = options;
                ImporterLogic.Import(ImporterConstants.DllPath);
                return Ok(ApiResource.Import_Success);

            }
            catch (ImporterNotFoundException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allImporters = ImporterLogic.GetAllImporters(ImporterConstants.DllPath);
            var serializedList = JsonConvert.SerializeObject(allImporters);
            return Ok(serializedList);
        }
    }
}