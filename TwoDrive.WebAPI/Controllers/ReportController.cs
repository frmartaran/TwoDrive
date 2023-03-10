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

        [HttpGet("Folder/Modifications")]
        public IActionResult GetFolderModificationReport([FromBody] DateTime start, [FromBody] DateTime end)
        {
            var folders = modificationLogic.GetAllFromDateRange(start, end)
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