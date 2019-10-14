using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Exceptions;
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
        private IModificationLogic modificationLogic;

        private ILogic<File> fileLogic;

        private const int TOP_WRITERS_COUNT = 10;
        public ReportController(IModificationLogic logic, ILogic<File> currentfileLogic)
        {
            modificationLogic = logic;
            fileLogic = currentfileLogic;
        }

        [HttpGet("File/Modifications")]
        public IActionResult GetFileModificationReport([FromBody] DateTime start, [FromBody] DateTime end)
        {
            try
            {
                var groups = modificationLogic.GetAllFromDateRange(start, end);
                if (groups.Count == 0)
                {
                    return Ok("There haven't been any modfications to files yet");
                }
                var report = groups
                    .Where(g => g.Key.GetType().IsSubclassOf(typeof(File)))
                    .Select(r => new ModificationReportModel
                    {
                        FileName = r.Key.Name,
                        Amount = r.Count()

                    }).ToList();
                return Ok(report);
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("Folder/Modifications")]
        public IActionResult GetFolderModificationsReport()
        {
            return null;
        }

        [HttpGet]
        public IActionResult GetTopWriters()
        {
            var allFiles = fileLogic.GetAll();
            if (allFiles.Count() == 0)
            {
                return Ok("There are no top writers yet");
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