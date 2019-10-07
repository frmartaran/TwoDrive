using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class GetTests
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
        public void Get()
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
                ParentFolder = folder,
                ParentFolderId = 1,
                CreationDate = new DateTime(2019, 6, 10),
                DateModified = new DateTime(2019, 6, 10),
            };

            var fileAsModel = new TxtModel().FromDomain(file);

            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IElementValidator>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);
            var result = controller.Get(1);
            var okResult = result as OkObjectResult;
            var modelResult = okResult.Value as TxtModel;
            var fileResult = modelResult.ToDomain();

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(file, fileResult);
        }

        [TestMethod]
        public void GetNotFound()
        {
            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<File>(null);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IElementValidator>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);
            var result = controller.Get(1);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void GetAllAdmin()
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
                OwnerId = 1,
                ParentFolder = folder,
                ParentFolderId = 1,
                CreationDate = new DateTime(2019, 6, 10),
                DateModified = new DateTime(2019, 6, 10),
            };
            var secondfile = new TxtFile
            {
                Id = 1,
                Name = "New second file",
                Content = "Content",
                Owner = writer,
                OwnerId = 1,
                ParentFolder = folder,
                ParentFolderId = 1,
                CreationDate = new DateTime(2019, 6, 10),
                DateModified = new DateTime(2019, 6, 10),
            };
            var files = new List<File>
            {
                file,
                secondfile
            };

            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAll())
                .Returns(files);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IElementValidator>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);
            var result = controller.GetAll(1);
            var okResult = result as OkObjectResult;
            var models = okResult.Value as List<FileModel>;
            var asTxtModels = models.Select(m => m as TxtModel);
            var filesResult = asTxtModels
                .Select(m => m.ToDomain())
                .ToList();

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue(filesResult.Contains(file));
            Assert.IsTrue(filesResult.Contains(secondfile));

        }

        [TestMethod]
        public void GetNotFoundAdmin()
        {
            var files = new List<File>();
            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAll())
                .Returns(files);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IElementValidator>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);
            var result = controller.GetAll(1);
            var okResult = result as NotFoundObjectResult;

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

        }

        [TestMethod]
        public void GetAllSessionWriter()
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
            var secondfile = new TxtFile
            {
                Id = 1,
                Name = "New second file",
                Content = "Content",
                Owner = writer,
                OwnerId = 2,
                ParentFolder = folder,
                ParentFolderId = 1,
                CreationDate = new DateTime(2019, 6, 10),
                DateModified = new DateTime(2019, 6, 10),
            };
            var files = new List<File>
            {
                file,
                secondfile
            };

            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAll())
                .Returns(files);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IElementValidator>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);
            var result = controller.GetAll();
            var okResult = result as OkObjectResult;
            var models = okResult.Value as List<FileModel>;
            var asTxtModels = models.Select(m => m as TxtModel);
            var filesResult = asTxtModels
                .Select(m => m.ToDomain())
                .ToList();

            mockLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue(filesResult.Contains(file));
            Assert.IsTrue(filesResult.Contains(secondfile));

        }

        [TestMethod]
        public void GetAllSessionNullWriter()
        {
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns<Writer>(null);

            var mockLogic = new Mock<ILogic<File>>();

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IElementValidator>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);
            var result = controller.GetAll();

            mockLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

        }

        [TestMethod]
        public void GetNoFilesSessionWriter()
        {
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var files = new List<File>();
            var mockLogic = new Mock<ILogic<File>>();
            mockLogic.Setup(m => m.GetAll())
                .Returns(files);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IElementValidator>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);
            var result = controller.GetAll();

            mockLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

        }
    }
}
