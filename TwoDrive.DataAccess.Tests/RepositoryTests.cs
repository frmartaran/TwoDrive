using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Domain;
using System;

namespace TwoDrive.DataAccess.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void GetInMemoryDatabase()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext1");
            Assert.IsNotNull(memoryDb);
        }

        [TestMethod]
        public void GetAllFilesAndFolders()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext18");
            var repository = new Repository<Element>(memoryDb);
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var file = new TxtFile
            {
                Id = 1,
                Name = "File",

            };
            var anotherFile = new TxtFile
            {
                Id = 2,
                Name = "File",

            };
            repository.Insert(file);
            repository.Insert(anotherFile);
            repository.Insert(folder);
            repository.Save();

            var all = repository.GetAll();
            var fileCount = all.Where(e => e is TxtFile).Count();
            var folderCount = all.Where(e => e is Folder).Count();

            Assert.AreEqual(3, all.Count());
            Assert.IsTrue(all.Contains(file));
            Assert.IsTrue(all.Contains(anotherFile));
            Assert.IsTrue(all.Contains(folder));
            Assert.AreEqual(1, folderCount);
            Assert.AreEqual(2, fileCount);
        }
        public void ExistsSession()
        {
            var token = Guid.NewGuid();
            var context = ContextFactory.GetMemoryContext("Session Test");
            var writer = new Writer
            {
                Id = 1
            };
            var session = new Session
            {
                Id = 2,
                Writer = writer,
                Token = token

            };
            var repository = new SessionRepository(context);
            repository.Insert(session);
            repository.Save();

            var exists = repository.Exists(session);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void ElementDbSet()
        {
            var context = ContextFactory.GetMemoryContext("Element Test");
            var repository = new Repository<Element>(context);
            
            var root = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var file = new TxtFile
            {
                Id = 4,
                Name = "Folder",
                Content = "",
                ParentFolder = root
            };
            repository.Insert(file);
            repository.Insert(root);
            repository.Save();
            Assert.IsTrue(context.Elements.Contains(file));
            Assert.IsTrue(context.Elements.Contains(root));
        }
    }
}