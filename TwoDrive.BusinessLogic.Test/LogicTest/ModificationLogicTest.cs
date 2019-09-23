using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class ModificationLogicTest
    {
        private Folder folder;

        [TestInitialize]
        public void SetUp()
        {
            folder = new Folder
            {
                Id = 1,
                Name = "Root",
                CreationDate = new DateTime(2019, 5, 23),
                DateModified = new DateTime(2019, 5, 23),
                Owner = new Writer(),
                FolderChilden = new List<Element>()
            };
        }

        [TestMethod]
        public void CreateModificationMock()
        {
            var mockRepository = new Mock<IRepository<Modification>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.Insert(It.IsAny<Modification>()));
            mockRepository.Setup(m => m.Save());
            var modification = new Modification
            {
                Id = 1,
                ElementModified = folder,
                type = ModificationType.Added,
                Date = folder.DateModified
            };
            var logic = new ModificationLogic(mockRepository.Object);
            logic.Create(modification);

            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void CreateModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification 1");
            var repository = new ModificationRepository(context);
            var modification = new Modification
            {
                Id = 1,
                ElementModified = folder,
                type = ModificationType.Added,
                Date = folder.DateModified
            };
            var logic = new ModificationLogic(repository);
            logic.Create(modification);

        }
    }
}