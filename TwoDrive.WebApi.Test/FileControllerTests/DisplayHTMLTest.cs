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
                Friends = new List<Writer>()
            };
        }

        [TestMethod]
        public void DisplayAsSimpleString()
        {
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = new Writer()
            };
            var content = "<html><h1>This is a test</h1></html>";
            var file = new HTMLFile
            {
                Id = 1,
                Name = "New file",
                Content = content,
                Owner = writer,
                ShouldRender = false,
                ParentFolder = folder,
                ParentFolderId = 1,
                CreationDate = new DateTime(2019, 6, 10),
                DateModified = new DateTime(2019, 6, 10),
            };

            var fileAsModel = new HTMLModel().FromDomain(file);

            var mockLogic = new Mock<IFileLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockSession = new Mock<ICurrent>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);
            var result = controller.DisplayContent(1);
            var okResult = result as OkObjectResult;
            var stringResult = okResult.Value as string;

            var wasEncoded = WasEncoded(content, stringResult);

            mockLogic.VerifyAll();
            Assert.IsTrue(wasEncoded);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        private bool WasEncoded(string original, string content)
        {
            var uncoded = HttpUtility.HtmlDecode(content);
            if (uncoded.Equals(original))
                return true;
            return false;
        }
    }
}
