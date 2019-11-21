using System;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Filters;
using TwoDrive.WebApi.Models;
using TwoDrive.WebApi.Resource;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    [AuthorizeFilter(Role.Administrator)]
    [EnableCors("CorsPolicy")]
    public class ReportController : ControllerBase
    {
        private IModificationLogic modificationLogic;

        private ILogic<File> fileLogic;

        private const int TOP_WRITERS_COUNT = 10;
        public ReportController(IModificationLogic logic, ILogic<File> currentfileLogic)
        {
            modificationLogic = logic;
            fileLogic = currentfileLogic;
        }

        [HttpPost("File/Modifications")]
        public IActionResult GetFileModificationReport([FromBody] DateRangeModel range)
        {
            try
            {
                var groups = modificationLogic.GetAllFromDateRange(range.StartDate, range.EndDate);
                if (groups.Count == 0)
                {
                    return Ok(ApiResource.NoModificationsYet_ReportController);
                }
                var report = groups
                    .Where(g => g.Key.GetType().IsSubclassOf(typeof(File)))
                    .Select(r => new
                    {
                        Element = r.Key,
                        Amount = r.Count()

                    }).ToList();

                var byOwner = report.GroupBy(g => g.Element.Owner)
                    .Select(r => new ModificationReportModel
                    {
                        Owner = r.Key.UserName,
                        Amount = r.Sum(ig => ig.Amount)

                    }).ToList();

                return Ok(byOwner);
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("Folder/Modifications")]
        public IActionResult GetFolderModificationReport([FromBody] DateRangeModel range)
        {
            var folders = modificationLogic.GetAllFromDateRange(range.StartDate, range.EndDate)
                .Where(g => g.Key is Folder)
                .Select(r => new
                {
                    Element = r.Key,
                    Amount = r.Count()

                }).ToList();

            var byOwner = folders
                .GroupBy(g => g.Element.Owner)
                .Select(r => new ModificationReportModel
                {
                    Owner = r.Key.UserName,
                    Amount = r.Sum(ig => ig.Amount)
                })
                .ToList();

            return Ok(byOwner);
        }

        [HttpGet]
        public IActionResult GetTopWriters()
        {
            var allFiles = fileLogic.GetAll();
            if (allFiles.Count() == 0)
            {
                return Ok(ApiResource.NoTopWriters_ReportController);
            }
            var filesGroupedByOwner = allFiles
                    .GroupBy(g => g.Owner)
                    .OrderByDescending(g => g.Count())
                    .ToList();
            var takeTopWriters = filesGroupedByOwner
                .Take(TOP_WRITERS_COUNT)
                .Select(r => new TopWriterModel
                {
                    Username = r.Key.UserName,
                    FileCount = r.Count()

                }).ToList();
            return Ok(takeTopWriters);

        }
    }
}