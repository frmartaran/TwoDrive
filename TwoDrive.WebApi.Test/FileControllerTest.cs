using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Controllers;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Test
{
    [TestClass]
    public class FileControllerTest
    {

        [TestMethod]
        public void CreateFile()
        {

            var writer = new Writer()
            {
                Id = 2,
                UserName = "Writer",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = writer
            };
            var file = new TxtFile
            {
                Id = 1,
                Name = "New file",
                Content = "Content",
                ParentFolderId = 1,
            };

            var fileAsModel = new TxtModel().FromDomain(file);
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(folder);
            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Update(It.IsAny<Writer>()));
            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<File>()));

            var mockModification = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockModification.Setup(m => m.Create(It.IsAny<Modification>()));

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);

            var result = controller.Create(1, fileAsModel);
            var okResult = result as OkObjectResult;

            mockLogic.VerifyAll();
            mockFolderLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            mockModification.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CreateFileIsNotOwner()
        {

            var writer = new Writer()
            {
                Id = 2,
                UserName = "Writer",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = new Writer()
            };
            var file = new TxtFile
            {
                Id = 1,
                Name = "New file",
                Content = "Content",
                ParentFolderId = 1,
            };

            var fileAsModel = new TxtModel().FromDomain(file);
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(folder);

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            var mockLogic = new Mock<ILogic<File>>();
            var mockModification = new Mock<IModificationLogic>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);

            var result = controller.Create(1, fileAsModel);
            var badRquestResult = result as BadRequestObjectResult;

            mockFolderLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual("You are not owner of this folder", badRquestResult.Value);
        }

        [TestMethod]
        public void CreateFileValidationError()
        {

            var writer = new Writer()
            {
                Id = 2,
                UserName = "Writer",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = writer
            };
            var file = new TxtFile
            {
                Id = 1,
                Name = "New file",
                Content = "Content",
                ParentFolderId = 1,
            };

            var fileAsModel = new TxtModel().FromDomain(file);
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(folder);

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<File>()))
                .Throws(new ValidationException(""));

            var mockModification = new Mock<IModificationLogic>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);

            var result = controller.Create(1, fileAsModel);

            mockLogic.VerifyAll();
            mockFolderLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void CreateFileWriterNull()
        {

            var writer = new Writer()
            {
                Id = 2,
                UserName = "Writer",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = writer
            };
            var file = new TxtFile
            {
                Id = 1,
                Name = "New file",
                Content = "Content",
                ParentFolderId = 1,
            };

            var fileAsModel = new TxtModel().FromDomain(file);
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns((Writer) null);
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(folder);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockLogic = new Mock<ILogic<File>>();

            var mockModification = new Mock<IModificationLogic>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);

            var result = controller.Create(1, fileAsModel);
            var badRquestResult = result as BadRequestObjectResult;

            mockFolderLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual("You must log in first", badRquestResult.Value);
        }
    }
}
