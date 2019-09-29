
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess.Tests
{
    [TestClass]
    public class FolderRepositoryTest
    {
        [TestMethod]
        public void AddAFolder()
        {

            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext3");
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChildren = new List<Element>()
            };

            var repository = new FolderRepository(memoryDb);
            repository.Insert(folder);
            repository.Save();

            var writerInDb = memoryDb.Set<Element>().FirstOrDefault();
            Assert.AreEqual(folder.Id, writerInDb.Id);
        }

        [TestMethod]
        public void FindFolder()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext7");
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var repository = new FolderRepository(memoryDb);
            repository.Insert(folder);
            repository.Save();

            var folderInMemory = repository.Get(1);
            Assert.AreEqual(folder, folderInMemory);
        }

        [TestMethod]
        public void UpdateFolder()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext9");
            var repository = new FolderRepository(memoryDb);
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            repository.Insert(folder);
            repository.Save();

            Assert.AreEqual("Root", folder.Name);

            folder.Name = " root ";
            repository.Update(folder);
            repository.Save();
            var foundFolder = repository.Get(1);

            Assert.AreEqual(" root ", foundFolder.Name);
            Assert.AreEqual(folder, foundFolder);
        }

        [TestMethod]
        public void DeleteFolder()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext13");
            var repository = new FolderRepository(memoryDb);
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            repository.Insert(folder);
            repository.Save();

            var countBeforeDeleting = memoryDb.Set<Element>().Count();
            Assert.AreEqual(1, countBeforeDeleting);

            repository.Delete(1);
            repository.Save();

            var countAfterDeleting = memoryDb.Set<Element>().Count();
            Assert.AreEqual(0, countAfterDeleting);
        }

        [TestMethod]
        public void GetAllFolders()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext15");
            var repository = new FolderRepository(memoryDb);
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var child = new Folder
            {
                Id = 2,
                Name = "Child",
                ParentFolder = folder,
                FolderChildren = new List<Element>()
            };
            repository.Insert(folder);
            repository.Insert(child);
            repository.Save();

            var all = repository.GetAll();
            Assert.AreEqual(2, all.Count());
            Assert.IsTrue(all.Contains(folder));
            Assert.IsTrue(all.Contains(child));
            Assert.IsTrue(all.All(f => f.GetType().Name == "Folder"));
        }

        [TestMethod]
        public void ExistsFolder()
        {
            var context = ContextFactory.GetMemoryContext("Folder Exists Test");
            var repository = new FolderRepository(context);
            var root = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var folder = new Folder
            {
                Id = 5,
                Name = "Folder",
                ParentFolder = root,
                FolderChildren = new List<Element>()
            };
            repository.Insert(root);
            repository.Insert(folder);
            repository.Save();

            var exists = repository.Exists(folder);
            Assert.IsTrue(exists);
        }

    }
}