
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.Domain;

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
                UserName = "Writer",
                Password = "A Password",
            };
            var mockLogic = new Mock<WriterLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Create(It.IsAny<Writer>()));
            var mockController = new WriterController(mockLogic.Object);

            var result = mockController.Post(writerModel);
            var createdResult = result as CreatedAtRouteResult;
            var modelResul = createdResult.Value as writerModel;

            mockLogic.VerifyAll();
        }
    }
}