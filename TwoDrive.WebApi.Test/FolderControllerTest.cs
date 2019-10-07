using Microsoft.AspNetCore.Http;
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
using TwoDrive.WebApi.Interfaces;
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
            var mockSessionLogic = new Mock<ICurrent>(MockBehavior.Strict);
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
            var mockSessionLogic = new Mock<ICurrent>(MockBehavior.Strict);
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
            var writer = new Writer 
            { 
                Id =3
            };
            var folderToMove = new Folder
            {
                Id = 1,
                Owner = writer,
                OwnerId = writer.Id
            };
            var folderDestination = new Folder
            {
                Id = 2,
                Owner = writer,
                OwnerId = writer.Id
            };

            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            var mockSessionLogic = new Mock<ICurrent>(MockBehavior.Strict);
            var mockElementRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            var mockElementValidator = new Mock<IElementValidator>(MockBehavior.Strict);

            mockSessionLogic.Setup(m => m.GetCurrentUser(It.IsAny<HttpContext>()))
            .Returns(writer);
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(folderToMove);
            mockFolderLogic.Setup(m => m.MoveElement(It.IsAny<Element>(), It.IsAny<Folder>(), It.IsAny<MoveElementDependencies>()));

            var controller = new FolderController(mockFolderLogic.Object, mockSessionLogic.Object,
                mockElementRepository.Object, mockElementValidator.Object);
            var result = controller.MoveFolder(folderToMove.Id, folderDestination.Id);

            mockFolderLogic.VerifyAll();
            mockSessionLogic.VerifyAll();
        }

        [TestMethod]
        public void UpdateFolder()
        {
            var writer = new Writer();
            var folder = new Folder
            {
                Id = 1,
                Owner = writer
            };

            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            var mockSessionLogic = new Mock<ICurrent>(MockBehavior.Strict);
            var mockElementRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            var mockElementValidator = new Mock<IElementValidator>(MockBehavior.Strict);

            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(folder);
            mockFolderLogic.Setup(m => m.Update(It.IsAny<Folder>()));

            var controller = new FolderController(mockFolderLogic.Object, mockSessionLogic.Object,
                mockElementRepository.Object, mockElementValidator.Object);

            var folderModel = new FolderModel().FromDomain(folder);
            var result = controller.Update(1, folderModel);
            var asOk = result as OkObjectResult;
            var folderModelResult = asOk.Value as FolderModel;
            var resultModel = folderModelResult.ToDomain();

            mockFolderLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(folder.Id, resultModel.Id);
        }

        [TestMethod]
        public void UpdateNullFolder()
        {
            var writerModel = new WriterModel
            {
                Id = 1
            };
            var model = new FolderModel
            {
                Id = 2,
                Name = "Name",
                OwnerId = writerModel.Id,
                Owner = writerModel
            };

            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            var mockSessionLogic = new Mock<ICurrent>(MockBehavior.Strict);
            var mockElementRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            var mockElementValidator = new Mock<IElementValidator>(MockBehavior.Strict);

            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns((Folder)null);
            mockFolderLogic.Setup(m => m.Update(It.IsAny<Folder>()))
            .Throws(new ValidationException(""));

            var controller = new FolderController(mockFolderLogic.Object, mockSessionLogic.Object,
                mockElementRepository.Object, mockElementValidator.Object);
            var result = controller.Update(1, model);

            mockFolderLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void GetFolder()
        {
            var writer = new Writer
            {
                Id = 1
            };
            var folder = new Folder
            {
                Id = 1,
                CreationDate = DateTime.Now,
                DateModified = DateTime.Now,
                FolderChildren = new List<Element>(),
                Name = "root",
                Owner = writer,
                OwnerId = writer.Id,
                ParentFolder = null,
                ParentFolderId = null
            };

            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            var mockSessionLogic = new Mock<ICurrent>(MockBehavior.Strict);
            var mockElementRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            var mockElementValidator = new Mock<IElementValidator>(MockBehavior.Strict);

            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(folder);

            var controller = new FolderController(mockFolderLogic.Object, mockSessionLogic.Object,
                mockElementRepository.Object, mockElementValidator.Object);

            var result = controller.Get(1);
            var asOk = result as OkObjectResult;
            var folderModelResult = asOk.Value as FolderModel;
            var resultModel = folderModelResult.ToDomain();

            mockFolderLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(folder.Id, resultModel.Id);
        }

        [TestMethod]
        public void GetNonExistantFolder()
        {
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            var mockSessionLogic = new Mock<ICurrent>(MockBehavior.Strict);
            var mockElementRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            var mockElementValidator = new Mock<IElementValidator>(MockBehavior.Strict);
            
            mockFolderLogic.Setup(m => m.Get(It.IsAny<int>()))
            .Returns((Folder)null);

            var controller = new FolderController(mockFolderLogic.Object, mockSessionLogic.Object,
                mockElementRepository.Object, mockElementValidator.Object);
            var result = controller.Get(1);

            mockFolderLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}
