
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Controllers;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Test
{
    [TestClass]
    public class WriterControllerTest
    {

        [TestMethod]
        public void ValidUser()
        {
            var writerModel = new WriterModel
            {
                Role = Role.Writer,
                UserName = "Valid Writer",
                Password = "1234",
                Friends = new List<Writer>(),
                Claims = new List<Claim>()
            };

            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<Writer>()));
            var mockFolderLogic = new Mock<ILogic<Folder>>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Create(It.IsAny<Folder>()));
            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object);
            var result = controller.Create(writerModel);

            mockLogic.VerifyAll();
            mockFolderLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void InvalidUser()
        {

            var writerModel = new WriterModel
            {
                Role = Role.Writer,
                Password = "1234",
                Friends = new List<Writer>(),
                Claims = new List<Claim>()
            };

            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<Writer>()))
            .Throws(new ValidationException(""));
            var mockFolderLogic = new Mock<ILogic<Folder>>(MockBehavior.Strict);

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object);
            var result = controller.Create(writerModel);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void DeleteWriter()
        {
            var writer = new Writer();
            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(writer);
            mockLogic.Setup(m => m.Delete(It.IsAny<int>()));
            
            var mockFolderLogic = new Mock<ILogic<Folder>>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Delete(It.IsAny<int>()));

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object);
            var result = controller.Delete(1);

            mockLogic.VerifyAll();
            mockFolderLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void DeleteNullWriter()
        {
            var writer = new Writer();
            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(writer);
            mockLogic.Setup(m => m.Delete(It.IsAny<int>()))
                .Throws(new LogicException(""));

            var mockFolderLogic = new Mock<ILogic<Folder>>(MockBehavior.Strict);

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object);
            var result = controller.Delete(1);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}