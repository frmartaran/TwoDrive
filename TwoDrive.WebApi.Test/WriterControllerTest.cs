
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
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
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
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);

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

            var root = new Folder();
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.GetRootFolder(It.IsAny<Writer>()))
                .Returns(root);
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

            var root = new Folder();
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.GetRootFolder(It.IsAny<Writer>()))
                .Returns(root);
            mockFolderLogic.Setup(m => m.Delete(It.IsAny<int>()));

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object);
            var result = controller.Delete(1);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void GetWriter()
        {
            var writer = new Writer
            {
                Role = Role.Writer,
                UserName = "Valid Writer",
                Password = "1234",
                Friends = new List<Writer>(),
                Claims = new List<Claim>()
            };
            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(writer);
            var mockFolderLogic = new Mock<IFolderLogic>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object);
            var result = controller.Get(1);
            var asOk = result as OkObjectResult;
            var writerModelResult = asOk.Value as WriterModel;
            var resultWriter = WriterModel.ToDomain(writerModelResult);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(writer, resultWriter);
        }

        [TestMethod]
        public void GetNonExistantWriter()
        {
            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns((Writer) null);

            var mockFolderLogic = new Mock<IFolderLogic>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object);
            var result = controller.Get(1);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void GetAll()
        {
            var writer = new Writer
            {
                Role = Role.Writer,
                UserName = "Valid Writer",
                Password = "1234",
                Friends = new List<Writer>(),
                Claims = new List<Claim>()
            };
            var friend = new Writer
            {
                Role = Role.Administrator,
                UserName = "Writer",
                Password = "1234",
                Friends = new List<Writer>(),
                Claims = new List<Claim>()
            };
            var writers = new List<Writer>
            {
                writer,
                friend
            };
            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAll())
                .Returns(writers);

            var mockFolderLogic = new Mock<IFolderLogic>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object);
            var result = controller.Get(1);
            var asOk = result as OkObjectResult;
            var writerModelResult = asOk.Value as List<WriterModel>;
            var resultWriter = WriterModel.AllToEntity(writerModelResult);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue(resultWriter.Contains(writer));
            Assert.IsTrue(resultWriter.Contains(friend));

        }


    }
}