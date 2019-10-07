using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
                .Returns<Writer>(null);
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

            var fileAsModel = new TxtModel().FromDomain(file);
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<Folder>(null);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockLogic = new Mock<ILogic<File>>();

            var mockModification = new Mock<IModificationLogic>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);

            var result = controller.Create(1, fileAsModel);
            var notFoundResult = result as NotFoundObjectResult;

            mockFolderLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual("Parent folder doesn't exist", notFoundResult.Value);
        }

        [TestMethod]
        public void Delete()
        {
            var file = new TxtFile
            {
                Id = 1,
                Name = "New file",
                Content = "Content",
                ParentFolderId = 1,
            };

            var mockSession = new Mock<ICurrent>();

            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);
            mockLogic.Setup(m => m.Delete(It.IsAny<int>()));

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockModification.Setup(m => m.Create(It.IsAny<Modification>()));
            var mockFolderLogic = new Mock<IFolderLogic>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);
            var result = controller.Delete(1);
            var okResult = result as OkObjectResult;

            mockLogic.VerifyAll();
            mockModification.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual($"{file.Name} has been deleted", okResult.Value);
        }

        [TestMethod]
        public void DeleteNull()
        {
            var mockSession = new Mock<ICurrent>();

            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<File>(null);
            mockLogic.Setup(m => m.Delete(It.IsAny<int>()))
                .Throws(new LogicException(""));

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);
            var result = controller.Delete(1);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
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


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);
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


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);
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


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);
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


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);
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


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);
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
                .Returns<Writer>(null);

            var mockLogic = new Mock<ILogic<File>>();

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object);
            var result = controller.GetAll();

            mockLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

        }
    }
}
