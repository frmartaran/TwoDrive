using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Controllers;
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
            var mockSessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
            var mockElementRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            var mockElementValidator = new Mock<IElementValidator>(MockBehavior.Strict);

            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(folder);
            mockFolderLogic.Setup(m => m.Delete(It.IsAny<int>()));

            var controller = new FolderController(mockFolderLogic.Object, mockSessionLogic.Object,
                mockElementRepository.Object, mockElementValidator.Object);
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
            var mockSessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
            var mockElementRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            var mockElementValidator = new Mock<IElementValidator>(MockBehavior.Strict);

            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(folder);
            mockFolderLogic.Setup(m => m.Delete(It.IsAny<int>()))
                .Throws(new LogicException(""));

            var controller = new FolderController(mockFolderLogic.Object, mockSessionLogic.Object,
                mockElementRepository.Object, mockElementValidator.Object);
            var result = controller.Delete(1);

            mockFolderLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void MoveFolder()
        {
            var writer = new Writer();
            var folder = new Folder
            {
                Id = 1,
                Owner = writer
            };

            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            var mockSessionLogic = new Mock<ISessionLogic>(MockBehavior.Strict);
            var mockElementRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            var mockElementValidator = new Mock<IElementValidator>(MockBehavior.Strict);

            mockSessionLogic.Setup(m => m.GetWriter(It.IsAny<string>()))
            .Returns(writer);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(folder);
            mockFolderLogic.Setup(m => m.MoveElement(It.IsAny<Element>(), It.IsAny<Folder>(), It.IsAny<MoveElementDependencies>()));
            mockFolderLogic.Setup(m => m.Delete(It.IsAny<int>()))
                .Throws(new LogicException(""));

            var controller = new FolderController(mockFolderLogic.Object, mockSessionLogic.Object,
                mockElementRepository.Object, mockElementValidator.Object);
            var result = controller.Delete(1);

            mockFolderLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
