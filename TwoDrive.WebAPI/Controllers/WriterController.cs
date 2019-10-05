
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Filters;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class WriterController : ControllerBase
    {
        private ILogic<Writer> Logic { get; set; }
        private IFolderLogic FolderLogic { get; set; }
        public WriterController(ILogic<Writer> logic, IFolderLogic folderLogic) : base()
        {
            Logic = logic;
            FolderLogic = folderLogic;
        }

        [HttpPost]
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult Create([FromBody] WriterModel model)
        {
            try
            {
                var writer = WriterModel.ToDomain(model);
                var root = new Folder
                {
                    Name = "Root",
                    CreationDate = DateTime.Now,
                    DateModified = DateTime.Now,
                    Owner = writer,
                    FolderChildren = new List<Element>()
                };
                writer.AddRootClaims(root);
                Logic.Create(writer);
                FolderLogic.Create(root);
                return Ok("Writer Created");
            }
            catch (ValidationException validationError)
            {
                return BadRequest(validationError.Message);
            }
        }

        [HttpDelete("{id}")]
        [AuthorizeFilter(Role.Administrator)]
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

        [HttpGet("{id}")]
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult Get(int id)
        {
            var writer = Logic.Get(id);
            if (writer == null)
            {
                return NotFound("User not found");
            }
            var model = WriterModel.FromDomain(writer);
            return Ok(model);
        }
    }
}