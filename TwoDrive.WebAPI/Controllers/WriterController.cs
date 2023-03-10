
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Filters;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class WriterController : ControllerBase
    {
        private ILogic<Writer> Logic { get; set; }
        private IFolderLogic FolderLogic { get; set; }
        private ICurrent CurrentSession { get; set; }
        public WriterController(ILogic<Writer> logic, IFolderLogic folderLogic, ICurrent current) : base()
        {
            Logic = logic;
            FolderLogic = folderLogic;
            CurrentSession = current;
        }

        [HttpPost]
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult Create([FromBody] WriterModel model)
        {
            if (model == null)
                return BadRequest("Something's went wrong in the request body");

            try
            {
                var writer = model.ToDomain();
                Logic.Create(writer);
                var root = new Folder
                {
                    Name = "Root",
                    Owner = writer,
                    CreationDate = DateTime.Now,
                    DateModified = DateTime.Now,
                    FolderChildren = new List<Element>()
                };
                FolderLogic.Create(root);
                writer.AddRootClaims(root);
                Logic.Update(writer);
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
            var model = new WriterModel();
            return Ok(model.FromDomain(writer));
        }

        [HttpGet]
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult Get()
        {
            var writers = Logic.GetAll()
                .ToList();
            if (writers.Count == 0)
            {
                return NotFound("No writers found");
            }
            var asModels = writers
                .Select(w => new WriterModel().FromDomain(w))
                .ToList();
            return Ok(asModels);
        }

        [HttpPut("{id}")]
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult Update(int id, [FromBody] WriterModel model)
        {
            if (model == null)
                return BadRequest("There's an error in the request");

            try
            {
                var writer = Logic.Get(id);
                writer = model.ToDomain(writer);
                Logic.Update(writer);
                var updatedWriter = Logic.Get(id);
                var toModel = new WriterModel();
                return Ok(toModel.FromDomain(writer));
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("Friend/{id}")]
        public IActionResult AddFriend(int id)
        {
            try
            {
                var writer = CurrentSession.GetCurrentUser(HttpContext);
                if (writer == null)
                    return BadRequest("You need to login first");
                var friend = Logic.Get(id);
                if (friend == null)
                    return BadRequest("The friend doesn't exist");

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

        [HttpDelete("Unfriend/{id}")]
        public IActionResult RemoveFriend(int id)
        {
            try
            {
                var writer = CurrentSession.GetCurrentUser(HttpContext);
                if (writer == null)
                    return BadRequest("You must log in first");
                var friend = Logic.Get(id);
                if (friend == null)
                    return BadRequest("The writer doesn't exist");
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

        [HttpGet("Friends/{id}")]
        public IActionResult ShowFriends(int id)
        {
            try
            {
                var writer = Logic.Get(id);
                if (writer.Friends.Count == 0)
                {
                    return Ok("Writer has no friends");
                }
                else
                {
                    var toModel = writer.Friends
                        .Select(f => new WriterModel().FromDomain(f))
                        .ToList();
                    return Ok(toModel);
                }
            }
            catch (NullReferenceException)
            {
                return BadRequest("Writer not found");
            }

        }
    }
}
