using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Filters;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    [AuthorizeFilter(Role.Administrator)]
    public class ReportController : ControllerBase
    {
        private IModificationLogic logic;
        public ReportController(IModificationLogic aLogic)
        {
            logic = aLogic;
        }

        [HttpGet]
        public IActionResult GetModificationReport(DateTime start, DateTime end)
        {
            var groups = logic.GetAllFromDateRange(start, end);
            var report = groups
                .Where(g => g.Key.GetType().IsSubclassOf(typeof(File)))
                .Select(r => new ModificationReportModel
                {
                    FileName = r.Key.Name,
                    Amount = r.Count()

                }).ToList();
            return Ok(report);
        }
    }
}