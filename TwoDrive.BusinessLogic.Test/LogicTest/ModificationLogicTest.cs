using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Logic;
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

        [TestMethod]
        public void GetModificationFromDateRangeMock()
        {
            var testList = new List<Modification>();
            var mockRepository = new Mock<IRepository<Modification>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.GetAll())
            .Returns(testList);
            var startDate = new DateTime(2019, 5, 4);
            var endDate = new DateTime(2019, 5, 5);
            var logic = new ModificationLogic(mockRepository.Object);
            var allModifications = logic.GetAllFromDateRange(startDate, endDate);

            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void GetModificationFromDateRange()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 2");
            var repository = new ModificationRepository(context);
            var startDate = new DateTime(2019, 5, 4);
            var endDate = new DateTime(2019, 5, 6);
            var modification = new Modification
            {
                Id = 2,
                ElementModified = folder,
                type = ModificationType.Added,
                Date = folder.DateModified
            };
            var anotherModification = new Modification
            {
                Id = 1,
                ElementModified = folder,
                type = ModificationType.Changed,
                Date = folder.DateModified
            };
            var logic = new ModificationLogic(repository);
            logic.Create(modification);
            logic.Create(anotherModification);

            var modificationsGroups = logic.GetAllFromDateRange(startDate, endDate);
            Assert.AreEqual(1, modificationsGroups.Count);
        }
    }
}