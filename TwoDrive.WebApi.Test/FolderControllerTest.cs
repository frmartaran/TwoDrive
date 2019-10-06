using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Test
{
    [TestClass]
    public class FolderControllerTest
    {
        [TestMethod]
        public void DeleteFolder()
        {
            var folder = new Folder
            {
                Id =1
            };

            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(folder);
            mockFolderLogic.Setup(m => m.Delete(It.IsAny<int>()));

            var controller = new FolderController(mockFolderLogic.Object);
            var result = controller.Delete(1);

            mockFolderLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void DeleteNullFolder()
        {
            var folder = new Folder
            {
                Id = 1
            };

            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(folder);
            mockFolderLogic.Setup(m => m.Delete(It.IsAny<int>()))
                .Throws(new LogicException(""));

            var controller = new FolderController(mockFolderLogic.Object);
            var result = controller.Delete(1);

            mockFolderLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
