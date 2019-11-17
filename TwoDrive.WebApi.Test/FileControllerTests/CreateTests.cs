using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Controllers;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Models;
using TwoDrive.WebApi.Resource;

namespace TwoDrive.WebApi.Test.FileControllerTests
{
    [TestClass]
    public class CreateTests
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
                Friends = new List<WriterFriend>()
            };
        }

        [TestMethod]
        public void CreateFile()
        {
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
                ParentFolder = null,
                ParentFolderId = 1,
            };

            var fileAsModel = new TxtModel().FromDomain(file) as TxtModel;
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(folder);
            mockFolderLogic.Setup(m => m.CreateModificationsForTree(It.IsAny<Element>(), 
                It.IsAny<ModificationType>()));
            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Update(It.IsAny<Writer>()));
            var mockLogic = new Mock<IFileLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<File>()));

            var mockModification = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockModification.Setup(m => m.Create(It.IsAny<Modification>()));

            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

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

            var fileAsModel = new TxtModel().FromDomain(file) as TxtModel;
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(folder);

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            var mockLogic = new Mock<IFileLogic>();
            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Create(1, fileAsModel);
            var badRquestResult = result as BadRequestObjectResult;

            mockFolderLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var message = string.Format(ApiResource.NotOwnerOf, folder.Name);
            Assert.AreEqual(message, badRquestResult.Value);
        }

        [TestMethod]
        public void CreateFileValidationError()
        {
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

            var fileAsModel = new TxtModel().FromDomain(file) as TxtModel;
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(folder);

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            var mockLogic = new Mock<IFileLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<File>()))
                .Throws(new ValidationException(""));

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Create(1, fileAsModel);

            mockLogic.VerifyAll();
            mockFolderLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void CreateFileWriterNull()
        {
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

            var fileAsModel = new TxtModel().FromDomain(file) as TxtModel;
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns<Writer>(null);
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(folder);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockLogic = new Mock<IFileLogic>();

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Create(1, fileAsModel);
            var badRquestResult = result as BadRequestObjectResult;

            mockFolderLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(ApiResource.MustLogIn, badRquestResult.Value);
        }

        [TestMethod]
        public void CreateFileNullFolder()
        {
            var file = new TxtFile
            {
                Id = 1,
                Name = "New file",
                Content = "Content",
                ParentFolderId = 1,
            };

            var fileAsModel = new TxtModel().FromDomain(file) as TxtModel;
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<Folder>(null);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockLogic = new Mock<IFileLogic>();

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Create(1, fileAsModel);
            var notFoundResult = result as NotFoundObjectResult;

            mockFolderLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(ApiResource.ParentFolderNotFound, notFoundResult.Value);
        }

    }
}
