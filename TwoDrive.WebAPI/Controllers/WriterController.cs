
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic;
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
        private ISessionLogic SessionLogic { get; set; }
        public WriterController(ILogic<Writer> logic, IFolderLogic folderLogic, ISessionLogic sessions) : base()
        {
            Logic = logic;
            FolderLogic = folderLogic;
            SessionLogic = sessions;
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

        [HttpGet]
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult Get()
        {
            var writers = Logic.GetAll();
            if (writers.Count == 0)
            {
                return NotFound("No writers found");
            }
            var asModels = WriterModel.AllToModel(writers);
            return Ok(asModels);
        }

        [HttpPut("{id}")]
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult Update(int id)
        {
            try
            {
                var writer = Logic.Get(id);
                Logic.Update(writer);
                var updatedWriter = Logic.Get(id);
                var toModel = WriterModel.FromDomain(updatedWriter);
                return Ok(toModel);
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        public IActionResult AddFriend(int id)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                var writer = SessionLogic.GetWriter(token);
                var friend = Logic.Get(id);
                if (writer.IsFriendsWith(friend))
                {
                    return BadRequest($"You're already friend with {friend.UserName}");
                };
                writer.Friends.Add(friend);
                Logic.Update(writer);
                return Ok($"You are now friends with {friend.UserName}");
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        public IActionResult RemoveFriend(int id)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"];
                var writer = SessionLogic.GetWriter(token);
                var friend = Logic.Get(id);
                if (!writer.IsFriendsWith(friend))
                {
                    return BadRequest("Can't remove friend since you aren't friends");
                };
                writer.Friends.Remove(friend);
                Logic.Update(writer);
                return Ok($"You are not friends with {friend.UserName} anymore");
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult ShowFriends(int id)
        {
            var writer = Logic.Get(id);
            var toModel = WriterModel.AllToModel(writer.Friends);
            return Ok(toModel);
        }

    }
}