using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.Domain;
using TwoDrive.WebApi.Controllers;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Test
{
    [TestClass]
    public class TokenControllerTest
    {
        [TestMethod]
        public void Login()
        {
            var logInModel = new LogInModel
            {
                Username = "Writer",
                Password = "1234"
            };
            var token = Guid.NewGuid();
            var mockCurrentSession = new Mock<ICurrent>();
            var mockLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(token);
            var controller = new TokenController(mockLogic.Object, mockCurrentSession.Object);
            var result = controller.LogIn(logInModel);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void CantLogin()
        {
            var logInModel = new LogInModel
            {
                Username = "Writer",
                Password = "1234"
            };
            var token = Guid.NewGuid();
            var mockCurrentSession = new Mock<ICurrent>();
            var mockLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((Guid?) null);
            var controller = new TokenController(mockLogic.Object, mockCurrentSession.Object);
            var result = controller.LogIn(logInModel);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void LogOut()
        {
            var token = Guid.NewGuid();
            var session = new Session
            {
                Token = token,
                Writer = new Writer()
            };
            var mockLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.RemoveSession(It.IsAny<Session>()));
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentSession(It.IsAny<HttpContext>()))
                .Returns(session);
            var controller = new TokenController(mockLogic.Object, mockSession.Object);
            var result = controller.LogOut();
            var okResult = result as OkObjectResult;

            mockLogic.VerifyAll();
            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual("Bye!", okResult.Value);
        }

        [TestMethod]
        public void CantLogOut()
        {
            var mockLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.RemoveSession(It.IsAny<Session>()))
                .Throws(new LogicException(""));
            var mockSession = new Mock<ICurrent>(MockBehavior.Strict);
            mockSession.Setup(m => m.GetCurrentSession(It.IsAny<HttpContext>()))
                .Returns((Session) null);
            var controller = new TokenController(mockLogic.Object, mockSession.Object);
            var result = controller.LogOut();
            var badRequestResult = result as BadRequestObjectResult;

            mockSession.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual("There was an error logging out", badRequestResult.Value);
        }
    }
}
