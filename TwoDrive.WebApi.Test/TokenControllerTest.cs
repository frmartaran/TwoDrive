using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic;
using TwoDrive.Domain;
using TwoDrive.WebApi.Controllers;
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
            var mockLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(token);
            var controller = new TokenController(mockLogic.Object);
            var result = controller.LogIn(logInModel);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
