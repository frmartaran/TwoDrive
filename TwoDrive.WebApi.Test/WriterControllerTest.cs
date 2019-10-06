using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic;
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
                Friends = new List<WriterModel>(),
                Claims = new List<ClaimModel>()
            };

            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<Writer>()));
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Create(It.IsAny<Folder>()));
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
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
                Friends = new List<WriterModel>(),
                Claims = new List<ClaimModel>()
            };

            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<Writer>()))
            .Throws(new ValidationException(""));
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
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
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
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
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
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
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
            var result = controller.Get(1);
            var asOk = result as OkObjectResult;
            var writerModelResult = asOk.Value as WriterModel;
            var resultWriter = writerModelResult.ToDomain();

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
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
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
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
            var result = controller.Get();
            var asOk = result as OkObjectResult;
            var writerModelResult = asOk.Value as List<WriterModel>;
            var resultWriter = writerModelResult
                .Select(wm => wm.ToDomain())
                .ToList();

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue(resultWriter.Contains(writer));
            Assert.IsTrue(resultWriter.Contains(friend));

        }

        [TestMethod]
        public void GetAllEmpty()
        {
            var writers = new List<Writer>();
            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAll())
                .Returns(writers);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
            var result = controller.Get();
            var asOk = result as OkObjectResult;

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

        }

        [TestMethod]
        public void UpdateWriter()
        {
            var writer = new Writer
            {
                Role = Role.Writer,
                UserName = "Valid Writer",
                Password = "12345",
                Friends = new List<Writer>(),
                Claims = new List<Claim>()
            };
            var toModel = new WriterModel().FromDomain(writer);
            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Update(It.IsAny<Writer>()));
            mockLogic.Setup(m => m.Get(It.IsAny<int>())).Returns(writer);
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
            var result = controller.Update(1, toModel);
            var asOk = result as OkObjectResult;
            var writerModelResult = asOk.Value as WriterModel;
            var resultWriter = writerModelResult.ToDomain();

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(writer, resultWriter);

        }

        [TestMethod]
        public void UpdateNullWriter()
        {
            var model = new WriterModel
            {
                Role = Role.Writer,
                UserName = "Valid Writer",
                Password = "1234",
                Friends = new List<WriterModel>(),
                Claims = new List<ClaimModel>()
            };
            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Update(It.IsAny<Writer>()))
                .Throws(new ValidationException(""));
            mockLogic.Setup(m => m.Get(It.IsAny<int>())).Returns((Writer) null);
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
            var result = controller.Update(1, model);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        }

        [TestMethod]
        public void ShowFriendList()
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
            writer.Friends.Add(friend);
            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(writer);

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
            var result = controller.ShowFriends(1);
            var asOk = result as OkObjectResult;
            var writerModelResult = asOk.Value as List<WriterModel>;
            var resultWriter = writerModelResult
                .Select(wm => wm.ToDomain())
                .ToList();

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsTrue(resultWriter.Contains(friend));

        }

        [TestMethod]
        public void ShowEmptyFriendList()
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
            var mockSessionLogic = new Mock<ISessionLogic>();
            var mockCurrentSession = new Mock<ICurrent>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
            var result = controller.ShowFriends(1);
            var asOk = result as OkObjectResult;

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual("Writer has no friends", asOk.Value);
        }

        [TestMethod]
        public void AddFriend()
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
            var mockCurrentSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockCurrentSession.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
                .Returns(writer);

            var mockLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(friend);
            mockLogic.Setup(m => m.Update(It.IsAny<Writer>()));

            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSessionLogic = new Mock<ISessionLogic>();

            var controller = new WriterController(mockLogic.Object, mockFolderLogic.Object,
                mockSessionLogic.Object, mockCurrentSession.Object);
            var result = controller.AddFriend(1);
            var okResult = result as OkObjectResult;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        }

    }
}