using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Controllers;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Test.FileControllerTests
{
    [TestClass]
    public class DisplayHTMLTest
    {
        Mock<ILogic<Writer>> MockWriterLogic;
        Mock<IModificationLogic> MockModification;
        Mock<IFolderLogic> MockFolderLogic;
        Mock<ICurrent> MockSession;
        Mock<IRepository<Element>> MockElementRepository;
        Mock<IFolderValidator> MockElementValidator;
        Mock<IFileLogic> MockLogic;

        [TestInitialize]
        public void SetUp()
        {
            MockWriterLogic = new Mock<ILogic<Writer>>();
            MockModification = new Mock<IModificationLogic>();
            MockFolderLogic = new Mock<IFolderLogic>();
            MockSession = new Mock<ICurrent>();
            MockElementRepository = new Mock<IRepository<Element>>();
            MockElementValidator = new Mock<IFolderValidator>();
            MockLogic = new Mock<IFileLogic>(MockBehavior.Strict);
        }

        [TestMethod]
        public void DisplayAsSimpleString()
        {
            var content = "<html><h1>This is a test</h1></html>";
            var file = new HTMLFile
            {
                Id = 1,
                Content = content,
                ShouldRender = false,
            };

            MockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var controller = new FileController(MockLogic.Object, MockFolderLogic.Object,
                MockWriterLogic.Object, MockSession.Object, MockModification.Object,
                MockElementValidator.Object, MockElementRepository.Object);

            var result = controller.DisplayContent(1);
            var okResult = result as OkObjectResult;
            var stringResult = okResult.Value as string;

            var wasEncoded = WasEncoded(content, stringResult);

            MockLogic.VerifyAll();
            Assert.IsTrue(wasEncoded);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void DisplayAsHTML()
        {
            var content = "<html><h1>This is a test</h1></html>";
            var file = new HTMLFile
            {
                Id = 1,
                Content = content,
                ShouldRender = true,
            };

            MockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var controller = new FileController(MockLogic.Object, MockFolderLogic.Object,
                MockWriterLogic.Object, MockSession.Object, MockModification.Object,
                MockElementValidator.Object, MockElementRepository.Object);

            var result = controller.DisplayContent(1);
            var okResult = result as OkObjectResult;
            var stringResult = okResult.Value as string;

            var wasEncoded = WasEncoded(content, stringResult);

            MockLogic.VerifyAll();
            Assert.IsFalse(wasEncoded);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }


        [TestMethod]
        public void FileIsNotAnHtmlFile()
        {
           
            var content = "I'm A TXT File";
            var file = new TxtFile
            {
                Id = 1,
                Content = content,
            };

            MockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var controller = new FileController(MockLogic.Object, MockFolderLogic.Object,
                MockWriterLogic.Object, MockSession.Object, MockModification.Object,
                MockElementValidator.Object, MockElementRepository.Object);
            var result = controller.DisplayContent(1);
            var okResult = result as OkObjectResult;
            var stringResult = okResult.Value as string;

            MockLogic.VerifyAll();
            Assert.AreEqual(file.Content, stringResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void FileNotFound()
        {
            MockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<File>(null);

            var controller = new FileController(MockLogic.Object, MockFolderLogic.Object,
                MockWriterLogic.Object, MockSession.Object, MockModification.Object,
                MockElementValidator.Object, MockElementRepository.Object);
            var result = controller.DisplayContent(1);

            MockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        private bool WasEncoded(string original, string content)
        {
            if (!content.Equals(original))
                return true;
            return false;
        }
    }
}
