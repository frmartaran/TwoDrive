using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.WebApi.Resource;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
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
        public IActionResult Import([FromBody] string path, string importType, int ownerId)
        {
            var owner = WriterLogic.Get(ownerId);
            if (owner == null)
                return BadRequest(ApiResource.WriterNotFound);
            try
            {
                var options = new ImportingOptions
                {
                    FilePath = path,
                    FileType = importType,
                    Owner = owner
                };
                ImporterLogic.Options = options;
                ImporterLogic.Import();
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
            return null;
        }
    }
}