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

        [TestMethod]
        public void DeleteParentFolder()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 2");
            var repository = new ElementRepository(context);
            repository.Insert(root);
            repository.Save();
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChilden = new List<Element>()
            };
            root.FolderChilden.Add(child);
            repository.Insert(child);
            repository.Update(root);
            repository.Save();

            var allFoldersInDb = repository.GetAll();
            Assert.IsTrue(allFoldersInDb.Contains(root));
            Assert.IsTrue(allFoldersInDb.Contains(child));

            var logic = new FolderLogic(repository);
            logic.Delete(root);

            allFoldersInDb = repository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
        }

        [TestMethod]
        public void DeleteParentWithTwoChilds()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 2");
            var repository = new ElementRepository(context);
            repository.Insert(root);
            repository.Save();
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChilden = new List<Element>()
            };
            var secondChild = new Folder
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child 2",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChilden = new List<Element>()
            };
            root.FolderChilden.Add(child);
            repository.Insert(child);
            repository.Insert(secondChild);
            repository.Update(root);
            repository.Save();

            var allFoldersInDb = repository.GetAll();
            Assert.IsTrue(allFoldersInDb.Contains(root));
            Assert.IsTrue(allFoldersInDb.Contains(child));
            Assert.IsTrue(allFoldersInDb.Contains(secondChild));

            var logic = new FolderLogic(repository);
            logic.Delete(root);

            allFoldersInDb = repository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
        }

        [TestMethod]
        public void DeleteParentWithAFolderAndFile()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 3");
            var repository = new ElementRepository(context);
            repository.Insert(root);
            repository.Save();
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChilden = new List<Element>()
            };
            var file = new TxtFile
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                Content = "Content"
            };
            root.FolderChilden.Add(child);
            repository.Insert(child);
            repository.Insert(file);
            repository.Update(root);
            repository.Save();

            var allFoldersInDb = repository.GetAll();
            Assert.IsTrue(allFoldersInDb.Contains(root));
            Assert.IsTrue(allFoldersInDb.Contains(child));
            Assert.IsTrue(allFoldersInDb.Contains(file));

            var logic = new FolderLogic(repository);
            logic.Delete(root);

            allFoldersInDb = repository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
        }

        [TestMethod]
        public void DeleteParentWithTwoLevelsOfChilds()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test 4");
            var repository = new ElementRepository(context);
            repository.Insert(root);
            repository.Save();
            var child = new Folder
            {
                Id = 2,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChilden = new List<Element>()
            };
            var secondChild = new Folder
            {
                Id = 3,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child 2",
                Owner = root.Owner,
                ParentFolder = root,
                FolderChilden = new List<Element>()
            };
            var grandson = new Folder
            {
                Id = 4,
                CreationDate = new DateTime(2019, 9, 22),
                DateModified = new DateTime(2019, 9, 22),
                Name = "child 3",
                Owner = root.Owner,
                ParentFolder = child,
                FolderChilden = new List<Element>()
            };
            root.FolderChilden.Add(child);
            child.FolderChilden.Add(grandson);
            repository.Insert(child);
            repository.Insert(secondChild);
            repository.Update(root);
            repository.Save();

            var allFoldersInDb = repository.GetAll();
            Assert.IsTrue(allFoldersInDb.Contains(root));
            Assert.IsTrue(allFoldersInDb.Contains(child));
            Assert.IsTrue(allFoldersInDb.Contains(secondChild));
            Assert.IsTrue(allFoldersInDb.Contains(grandson));

            var logic = new FolderLogic(repository);
            logic.Delete(root);

            allFoldersInDb = repository.GetAll();
            Assert.AreEqual(0, allFoldersInDb.Count);
        }

        [TestMethod]
        public void UpdateFolder()
        {
            var mockRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            var mockValidator = new Mock<IValidator<Element>>(MockBehavior.Strict);

            mockRepository.Setup(m => m.Update(It.IsAny<Element>()));
            mockRepository.Setup(m => m.Save());
            mockValidator.Setup(m => m.isValid(It.IsAny<Element>()))
            .Returns(true);

            root.Name = "Root 2.0";
            var logic = new FolderLogic(mockRepository.Object, mockValidator.Object);
            logic.Update(root);
            mockRepository.VerifyAll();
            mockValidator.VerifyAll();

        }

        [TestMethod]
        public void UpdateFolderCheckState()
        {
            var context = ContextFactory.GetMemoryContext("Update Test");
            var repository = new ElementRepository(context);
            var validator = new FolderValidator();
            repository.Insert(root);
            repository.Save();

            var newOwner = new Writer();

            var dateModified = root.DateModified;
            root.Owner = newOwner;
            var logic = new FolderLogic(repository, validator);
            logic.Update(root);

            var folderInDb = repository.Get(1);

            Assert.AreNotEqual(dateModified, folderInDb.DateModified);
            Assert.AreEqual(newOwner, folderInDb.Owner);
        }

        [TestMethod]
        public void GetAll()
        {
            var testList = new List<Element>();
            var mockRepository = new Mock<IRepository<Element>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.GetAll())
            .Returns(testList);

            var logic = new FolderLogic(mockRepository.Object);
            var elements = logic.GetAll();

            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void GetAllFolders()
        {
            var context = ContextFactory.GetMemoryContext("Get All test");
            var repository = new ElementRepository(context);
            var newFolder = new Folder{
                Id = 2
            };
            var file = new TxtFile{
                Id = 3
            };
            repository.Insert(root);
            repository.Insert(newFolder);
            repository.Insert(file);
            repository.Save();

            var logic = new FolderLogic(repository);
            var allFoldersInDb = logic.GetAll();
            Assert.IsTrue(allFoldersInDb.Contains(root));
            Assert.IsTrue(allFoldersInDb.Contains(newFolder));
            Assert.AreEqual(2, allFoldersInDb.Count);


        }
    }
}