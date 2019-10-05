
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.Domain;
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

            var mockLogic = new Mock<WriterLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<Writer>()));
            var mockController = new WriterController(mockLogic.Object);

            var result = mockController.Post(writerModel);
            var createdResult = result as CreatedAtRouteResult;
            var modelResul = createdResult.Value as WriterModel;

            mockLogic.VerifyAll();
        }
    }
}