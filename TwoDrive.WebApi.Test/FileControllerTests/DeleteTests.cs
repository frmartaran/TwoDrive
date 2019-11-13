using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Controllers;
using TwoDrive.WebApi.Interfaces;
using TwoDrive.WebApi.Resource;

namespace TwoDrive.WebApi.Test.FileControllerTests
{
    [TestClass]
    public class DeleteTests
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
                Friends = new List<WriterFriend>()
            };
        }


        [TestMethod]
        public void Delete()
        {
            var root = new Folder
            {
                Id = 2,
                Name = "Folder",
                Owner = writer,
                ParentFolder = null,
                FolderChildren = new List<Element>()
            };
            
            var file = new TxtFile
            {
                Id = 1,
                Name = "New file",
                Content = "Content",
                ParentFolder = root,
            };

            root.FolderChildren.Add(file);

            var mockSession = new Mock<ICurrent>();

            var mockLogic = new Mock<IFileLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(file);
            mockLogic.Setup(m => m.Delete(It.IsAny<int>()));

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockModification.Setup(m => m.Create(It.IsAny<Modification>()));
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();

            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Delete(1);
            var okResult = result as OkObjectResult;

            mockLogic.VerifyAll();
            mockModification.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var message = string.Format(ApiResource.Delete_FileController, file.Name);
            Assert.AreEqual(message, okResult.Value);
        }

        [TestMethod]
        public void DeleteNull()
        {
            var mockSession = new Mock<ICurrent>();

            var mockLogic = new Mock<IFileLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<File>(null);
            mockLogic.Setup(m => m.Delete(It.IsAny<int>()))
                .Throws(new LogicException(""));

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModification = new Mock<IModificationLogic>();
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockElementRepository = new Mock<IRepository<Element>>();
            var mockElementValidator = new Mock<IFolderValidator>();


            var controller = new FileController(mockLogic.Object, mockFolderLogic.Object,
                mockWriterLogic.Object, mockSession.Object, mockModification.Object,
                mockElementValidator.Object, mockElementRepository.Object);

            var result = controller.Delete(1);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

    }
}
