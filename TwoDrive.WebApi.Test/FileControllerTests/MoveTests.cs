using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Controllers;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Test
{
    [TestClass]
    public class MoveTests
    {
        private Writer writer;
        [TestInitialize]
        public void SetUp()
        {
            writer = new Writer()
            {
                Id = 2,
                UserName = "Writer",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<Writer>()
            };
        }
        
        [TestMethod]
        public void Move()
        {
            var file = new TxtFile
            {
                Id = 1,
                Name = "New file",
                Content = "Content",
                Owner = writer,
                OwnerId = 2,
                ParentFolder = new Folder(),
                ParentFolderId = 1,
                CreationDate = new DateTime(2019, 6, 10),
                DateModified = new DateTime(2019, 6, 10),
            };
            var folderDestination = new Folder
            {
                Id = 2,
                Owner = writer,
                OwnerId = writer.Id
            };

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockModification = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockModification.Setup(m => m.Create(It.IsAny<Modification>()));

            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
            .Returns(writer);

            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(folderDestination);
            mockFolderLogic.Setup(m => m.MoveElement(It.IsAny<Element>(), It.IsAny<Folder>(),
                It.IsAny<MoveElementDependencies>()));

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Move(1, 2);
            mockFolderLogic.VerifyAll();
            mockLogic.VerifyAll();
            mockModification.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

        [TestMethod]
        public void MoveNullOwner()
        {

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockLogic = new Mock<ILogic<File>>();

            var mockModification = new Mock<IModificationLogic>();

            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
            .Returns<Writer>(null);

            var mockFolderLogic = new Mock<IFolderLogic>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Move(1, 2);
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

        }

        [TestMethod]
        public void MoveNullFile()
        {

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockLogic = new Mock<ILogic<File>>();
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<File>(null);

            var mockModification = new Mock<IModificationLogic>();

            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
            .Returns(writer);

            var mockFolderLogic = new Mock<IFolderLogic>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Move(1, 2);
            mockSession.VerifyAll();
            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

        }

        [TestMethod]
        public void MoveNullFolder()
        {
            var file = new TxtFile
            {
                Id = 1,
                Name = "New file",
                Content = "Content",
                Owner = writer,
                OwnerId = 2,
                ParentFolder = new Folder(),
                ParentFolderId = 1,
                CreationDate = new DateTime(2019, 6, 10),
                DateModified = new DateTime(2019, 6, 10),
            };

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockLogic = new Mock<ILogic<File>>();
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockModification = new Mock<IModificationLogic>();

            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
            .Returns(writer);

            var mockFolderLogic = new Mock<IFolderLogic>();
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<Folder>(null);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Move(1, 2);
            mockSession.VerifyAll();
            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

        }

        [TestMethod]
        public void MoveNotOwner()
        {
            var file = new TxtFile
            {
                Id = 1,
                Name = "New file",
                Content = "Content",
                Owner = new Writer(),
                OwnerId = 2,
                ParentFolder = new Folder(),
                ParentFolderId = 1,
                CreationDate = new DateTime(2019, 6, 10),
                DateModified = new DateTime(2019, 6, 10),
            };
            var folderDestination = new Folder
            {
                Id = 2,
                Owner = writer,
                OwnerId = writer.Id
            };

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockModification = new Mock<IModificationLogic>();

            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
            .Returns(writer);

            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(folderDestination);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Move(1, 2);
            mockFolderLogic.VerifyAll();
            mockLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        }
    }
}
