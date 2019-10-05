
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.Domain;
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
            var controller = new WriterController(mockLogic.Object);

            var result = controller.Create(writerModel);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}