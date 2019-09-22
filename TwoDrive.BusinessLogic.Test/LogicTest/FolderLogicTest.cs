using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class FolderLogicTest
    {
        private Folder root;

        [TestInitialize]
        public void SetUp()
        {
            var writer = new Writer();
            root = new Folder
            {
                Id = 1,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "Root",
                Owner = writer,
                FolderChilden = new List<Element>()
            };

        }

        [TestMethod]
        public void CreateFolder()
        {
            var mockRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.Insert(It.IsAny<Element>()));
            mockRepository.Setup(m => m.Save());

            var mockValidator = new Mock<IValidator<Element>>(MockBehavior.Strict);
            mockValidator.Setup(m => m.isValid(It.IsAny<Element>()))
            .Returns(true);

            var logic = new FolderLogic(mockRepository.Object, mockValidator.Object);
            logic.Create(root);

            mockRepository.VerifyAll();
            mockValidator.VerifyAll();
        }

        [TestMethod]
        public void CreateFolderCheckState()
        {
            var context = ContextFactory.GetMemoryContext("Create Test");
            var repository = new ElementRepository(context);
            var validator = new FolderValidator();
            var logic = new FolderLogic(repository, validator);

            logic.Create(root);

            var folderinDb = repository.Get(1);
            Assert.AreEqual(root, folderinDb);
        }

        [TestMethod]
        public void DeleteFolder()
        {
            var mockRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.Delete(It.IsAny<int>()));
            mockRepository.Setup(m => m.Save());

            var logic = new FolderLogic(mockRepository.Object);
            logic.Delete(root);

            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void DeleteOneFolder()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test");
            var repository = new ElementRepository(context);
            repository.Insert(root);
            repository.Save();

            var rootInDb = repository.Get(1);
            Assert.AreEqual(root, rootInDb);

            var logic = new FolderLogic(repository);
            logic.Delete(root);

            rootInDb = repository.Get(1);
            Assert.IsNull(rootInDb);    
        }


    }
}