using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Logic;
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

            var mockValidator = new Mock<IValidator<Folder>>(MockBehavior.Strict);
            mockValidator.Setup(m => m.isValid(It.IsAny<Folder>()))
            .Returns(true);

            var logic = new FolderLogic(mockRepository.Object, mockValidator.Object);
            logic.Create(root);

            mockRepository.VerifyAll();
            mockValidator.VerifyAll();
        }
    }
}