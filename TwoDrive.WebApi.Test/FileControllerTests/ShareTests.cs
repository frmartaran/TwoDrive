using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Controllers;
using TwoDrive.WebApi.Interfaces;

namespace TwoDrive.WebApi.Test.FileControllerTests
{
    [TestClass]
    public class ShareTests
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
        public void Share()
        {
            var friend = new Writer
            {
                Id = 3,
                UserName = "Friend",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<WriterFriend>()
            };
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
            var writerFriend = new WriterFriend
            {
                Writer = writer,
                Friend = friend
            };
            writer.Friends.Add(writerFriend);
            var mockLogic = new Mock<IFileLogic>();
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Update(It.IsAny<Writer>()));
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(friend);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);


            var result = controller.Share(1, 3);

            mockLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(1, friend.Claims.Count);
        }

        [TestMethod]
        public void ShareNullWriter()
        {
            var mockLogic = new Mock<IFileLogic>();

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>();

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns<Writer>(null);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Share(1, 3);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void ShareNullFriend()
        {
            var mockLogic = new Mock<IFileLogic>();

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<Writer>(null);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Share(1, 3);

            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void ShareNullFile()
        {
            var friend = new Writer
            {
                Id = 3,
                UserName = "Friend",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<WriterFriend>()
            };
            var writerFriend = new WriterFriend
            {
                Writer = writer,
                Friend = friend
            };
            writer.Friends.Add(writerFriend);
            var mockLogic = new Mock<IFileLogic>();
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<File>(null);

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(friend);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Share(1, 3);

            mockLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void ShareWithStranger()
        {
            var friend = new Writer
            {
                Id = 3,
                UserName = "Friend",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<WriterFriend>()
            };
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

            var mockLogic = new Mock<IFileLogic>();
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(friend);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Share(1, 3);

            mockLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void StopSharing()
        {

            var friend = new Writer
            {
                Id = 3,
                UserName = "Friend",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<WriterFriend>()
            };
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
            var writerFriend = new WriterFriend
            {
                Writer = writer,
                Friend = friend
            };
            writer.Friends.Add(writerFriend);
            writer.AllowFriendTo(friend, file, ClaimType.Read);

            var mockLogic = new Mock<IFileLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Update(It.IsAny<Writer>()));
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(friend);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.StopShare(1, 3);

            mockLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(0, friend.Claims.Count);
        }

        [TestMethod]
        public void StopSharingNullWriter()
        {

            var mockLogic = new Mock<IFileLogic>();

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>();

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns<Writer>(null);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.StopShare(1, 3);

            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void StopSharingNullFriend()
        {

            var mockLogic = new Mock<IFileLogic>();

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<Writer>(null);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.StopShare(1, 3);

            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void StopSharingNullFile()
        {

            var friend = new Writer
            {
                Id = 3,
                UserName = "Friend",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<WriterFriend>()
            };

            var writerFriend = new WriterFriend
            {
                Writer = writer,
                Friend = friend
            };
            writer.Friends.Add(writerFriend);

            var mockLogic = new Mock<IFileLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<File>(null);

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(friend);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.StopShare(1, 3);

            mockLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void StopSharingStranger()
        {

            var friend = new Writer
            {
                Id = 3,
                UserName = "Friend",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<WriterFriend>()
            };
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

            var mockLogic = new Mock<IFileLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(friend);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.StopShare(1, 3);

            mockLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void StopSharingFriendHasNoClaim()
        {
            var friend = new Writer
            {
                Id = 3,
                UserName = "Friend",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<WriterFriend>()
            };
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
            var writerFriend = new WriterFriend
            {
                Writer = writer,
                Friend = friend
            };
            writer.Friends.Add(writerFriend);

            var mockLogic = new Mock<IFileLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(friend);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.StopShare(1, 3);

            mockLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void ShareFriendHasClaim()
        {
            var friend = new Writer
            {
                Id = 3,
                UserName = "Friend",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<WriterFriend>()
            };
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
            var writerFriend = new WriterFriend
            {
                Writer = writer,
                Friend = friend
            };
            writer.Friends.Add(writerFriend);
            writer.AllowFriendTo(friend, file, ClaimType.Read);
            var mockLogic = new Mock<IFileLogic>();
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockModification = new Mock<IModificationLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(friend);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Share(1, 3);

            mockLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

    }
}
