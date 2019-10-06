using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.Domain;
using TwoDrive.WebApi.Filters;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    [AuthorizeFilter(Role.Administrator)]
    public class ReportController : ControllerBase
    {
        private ModificationLogic logic;
        public ReportController(ModificationLogic aLogic)
        {
            logic = aLogic;
        }

        [HttpGet]
        public IActionResult GetModificationReport(DateTime start, DateTime end)
        {
            return null;
        }
    }
}