using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private ImporterDependencies Dependencies;

        public ImportController(ImporterDependencies dependencies)
        {
            Dependencies = dependencies;
        }

        [HttpPost("{importType}/{ownerId}")]
        public IActionResult Import([FromBody] string path, string importType, int ownerId)
        {
            return null;
        }
    }
}