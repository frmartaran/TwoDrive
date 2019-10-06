﻿using System;
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

        [HttpGet]
        public IActionResult GetModificationReport(DateTime start, DateTime end)
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

        [HttpGet]
        public IActionResult GetTopWriters()
        {
            return null;
        }
    }
}