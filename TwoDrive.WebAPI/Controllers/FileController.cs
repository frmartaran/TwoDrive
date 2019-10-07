﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Filters;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private ICurrent inSession;

        private IFolderLogic folderLogic;

        private ILogic<File> fileLogic;

        private ILogic<Writer> writerLogic;

        private IModificationLogic modificationLogic;

        private IElementValidator elementValidator;

        private IRepository<Element> elementRepository;

        public FileController(ILogic<File> logicFile, IFolderLogic logicFolder,
            ILogic<Writer> logicWriter, ICurrent session, IModificationLogic logic,
            IElementValidator validator, IRepository<Element> repository)
        {
            inSession = session;
            folderLogic = logicFolder;
            fileLogic = logicFile;
            writerLogic = logicWriter;
            modificationLogic = logic;
            elementValidator = validator;
            elementRepository = repository;
        }

        [HttpPost("{Id}")]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult Create(int Id, [FromBody] TxtModel model)
        {
            try
            {
                var loggedWriter = inSession.GetCurrentUser(HttpContext);
                var parentFolder = folderLogic.Get(Id);
                if (loggedWriter == null)
                    return BadRequest("You must log in first");
                if (parentFolder == null)
                    return NotFound("Parent folder doesn't exist");
                if (loggedWriter != parentFolder.Owner)
                    return BadRequest("You are not owner of this folder");


                var file = model.ToDomain() as File;
                file.Owner = loggedWriter;
                file.ParentFolder = parentFolder;
                file.CreationDate = DateTime.Now;
                file.DateModified = DateTime.Now;
                fileLogic.Create(file);
                loggedWriter.AddCreatorClaimsTo(file);
                writerLogic.Update(loggedWriter);

                CreateModification(file, ModificationType.Added);
                return Ok(new TxtModel().FromDomain(file));
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}")]
        [ClaimFilter(ClaimType.Delete)]
        public IActionResult Delete(int id)
        {
            try
            {
                var file = fileLogic.Get(id);
                fileLogic.Delete(id);
                CreateModification(file, ModificationType.Deleted);
                return Ok($"{file.Name} has been deleted");
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet("{id}")]
        [ClaimFilter(ClaimType.Read)]
        public IActionResult Get(int id)
        {
            var file = fileLogic.Get(id);
            if (file == null)
                return NotFound("File not found");

            var model = new TxtModel().FromDomain(file);
            return Ok(model);
        }

        [HttpGet("Admin/{id}")]
        [AuthorizeFilter(Role.Administrator)]
        public IActionResult GetAll(int writerId)
        {
            var files = fileLogic.GetAll();
            var writerfiles = files
                .Where(f => f.OwnerId == writerId)
                .Select(f => new TxtModel().FromDomain(f))
                .ToList();

            if (writerfiles.Count == 0)
                return NotFound("No files found");

            return Ok(writerfiles);

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var writer = inSession.GetCurrentUser(HttpContext);
            if (writer == null)
                return NotFound("You need to log in first");

            var files = fileLogic.GetAll();
            var writerfiles = files
                .Where(f => f.OwnerId == writer.Id)
                .Select(f => new TxtModel().FromDomain(f))
                .ToList();

            if (writerfiles.Count == 0)
                return NotFound("No files found");

            return Ok(writerfiles);
        }

        [HttpPut("{id}")]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult Update(int id, [FromBody] TxtModel model)
        {
            try
            {
                var file = fileLogic.Get(id);
                var updatedFile = model.ToDomain();
                file = updatedFile;
                fileLogic.Update(file);
                CreateModification(file, ModificationType.Changed);
                return Ok("File Updated");
            }
            catch (ValidationException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut("{id}/{friendId}")]
        [ClaimFilter(ClaimType.Share)]
        public IActionResult Share(int id, int friendId)
        {
            try
            {
                var writer = inSession.GetCurrentUser(HttpContext);
                if (writer == null)
                    return NotFound("You must log in first");

                var friend = writerLogic.Get(friendId);
                if (friend == null)
                    return NotFound("Friend not found");

                var file = fileLogic.Get(id);
                if (file == null)
                    return NotFound("File not found");
                if (!writer.IsFriendsWith(friend))
                    return BadRequest($"You are not friends with {friend.UserName}");

                writer.AllowFriendTo(friend, file, ClaimType.Read);
                writerLogic.Update(friend);
                return Ok($"{file.Name} shared with {friend.UserName}");
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}/{friendId}")]
        [ClaimFilter(ClaimType.Share)]
        public IActionResult StopShare(int id, int friendId)
        {
            try
            {
                var writer = inSession.GetCurrentUser(HttpContext);
                if (writer == null)
                    return NotFound("You must log in first");

                var friend = writerLogic.Get(friendId);
                if (friend == null)
                    return NotFound("Friend not found");

                var file = fileLogic.Get(id);
                if (file == null)
                    return NotFound("File not found");
                if (!writer.IsFriendsWith(friend))
                    return BadRequest($"You must be friends with {friend.UserName} to revoke");

                writer.RevokeFriendFrom(friend, file, ClaimType.Read);
                writerLogic.Update(friend);
                return Ok($"Stop sharing file: {file.Name} with {friend.UserName}");
            }
            catch (LogicException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("{id}/{folderId}")]
        [ClaimFilter(ClaimType.Write)]
        public IActionResult Move(int id, int folderId)
        {
            var writer = inSession.GetCurrentUser(HttpContext);
            if (writer == null)
                return NotFound("You must log in first");

            var file = fileLogic.Get(id);
            if (file == null)
                return NotFound("File not found");

            var folder = folderLogic.Get(folderId);
            if (folder == null)
                return NotFound("Folder not found");

            if (!writer.IsOwnerOfOriginAndDestination(file, folder))
                return BadRequest("Writer is not owner of both the origin " +
                    "element and destiny folder");

            var dependencies = new MoveElementDependencies
            {
                ElementRepository = elementRepository,
                ElementValidator = elementValidator
            };
            folderLogic.MoveElement(file, folder, dependencies);
            CreateModification(file, ModificationType.Changed);
            return Ok($"File: {file.Name} moved to {folder.Name}");
        }

        private void CreateModification(File file, ModificationType action)
        {
            var modification = new Modification
            {
                ElementModified = file,
                type = action,
                Date = file.CreationDate
            };
            modificationLogic.Create(modification);
        }
    }
}