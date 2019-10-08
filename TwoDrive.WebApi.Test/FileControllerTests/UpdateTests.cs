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

namespace TwoDrive.WebApi.Test.FileControllerTests
{
    [TestClass]
    public class UpdateTests
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
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
        }
        [TestMethod]
        public void Update()
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
                Owner = writer,
                OwnerId = 2,
                ParentFolder = folder,
                ParentFolderId = 1,
                CreationDate = new DateTime(2019, 6, 10),
                DateModified = new DateTime(2019, 6, 10),
            };
            var model = new TxtModel().FromDomain(file) as TxtModel;

            var mockLogic = new Mock<IFileLogic>();
            mockLogic.Setup(m => m.Update(It.IsAny<File>()));
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockModification = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockModification.Setup(m => m.Create(It.IsAny<Modification>()));

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IElementValidator>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);
            var result = controller.Update(1, model);
            var okResult = result as OkObjectResult;

            mockLogic.VerifyAll();
            mockModification.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual("File Updated", okResult.Value);
        }

        [TestMethod]
        public void UpdateBadRequest()
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
                Owner = writer,
                OwnerId = 2,
                ParentFolder = folder,
                ParentFolderId = 1,
                CreationDate = new DateTime(2019, 6, 10),
                DateModified = new DateTime(2019, 6, 10),
            };
            var model = new TxtModel().FromDomain(file) as TxtModel;
            var mockLogic = new Mock<IFileLogic>();
            mockLogic.Setup(m => m.Update(It.IsAny<File>()))
                .Throws(new ValidationException(""));
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<File>(null);

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IElementValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);
            var result = controller.Update(1, model);
            var okResult = result as BadRequestObjectResult;

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
