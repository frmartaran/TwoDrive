
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess.Tests
{
    [TestClass]
    public class FileRepositoryTest
    {
        [TestMethod]
        public void AddAFile()
        {

            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext4");
            File file = new TxtFile
            {
                Id = 1,
                Name = "File",
            };

            var repository = new FileRepository(memoryDb);
            repository.Insert(file);
            repository.Save();

            var writerInDb = memoryDb.Set<Element>().FirstOrDefault();
            Assert.AreEqual(file.Id, writerInDb.Id);
        }
        [TestMethod]
        public void FindTxtFile()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext6");
            var file = new TxtFile
            {
                Id = 1,
                Name = "File",

            };
            var fileRepository = new FileRepository(memoryDb);
            fileRepository.Insert(file);
            fileRepository.Save();

            var fileInMemory = fileRepository.Get(1);
            Assert.AreEqual(file, fileInMemory);
        }

        [TestMethod]
        public void UpdateFile()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext10");
            var repository = new FileRepository(memoryDb);
            var file = new TxtFile
            {
                Id = 1,
                Name = "File",

            };
            repository.Insert(file);
            repository.Save();

            Assert.AreEqual("File", file.Name);

            file.Name = "TXT";
            repository.Update(file);
            repository.Save();
            var foundFile = repository.Get(1);

            Assert.AreEqual("TXT", foundFile.Name);
            Assert.AreEqual(file, foundFile);
        }

        [TestMethod]
        public void DeleteFile()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext12");
            var repository = new FileRepository(memoryDb);
            var file = new TxtFile
            {
                Id = 1,
                Name = "File",

            };
            repository.Insert(file);
            repository.Save();

            var countBeforeDeleting = memoryDb.Set<Element>().Count();
            Assert.AreEqual(1, countBeforeDeleting);

            repository.Delete(1);
            repository.Save();

            var countAfterDeleting = memoryDb.Set<Element>().Count();
            Assert.AreEqual(0, countAfterDeleting);
        }

        [TestMethod]
        public void GetAllFiles()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext16");
            var repository = new FileRepository(memoryDb);
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
            repository.Save();

            var all = repository.GetAll();
            Assert.AreEqual(2, all.Count());
            Assert.IsTrue(all.Contains(file));
            Assert.IsTrue(all.Contains(anotherFile));
            Assert.IsTrue(all.All(f => f.GetType().Name == "TxtFile"));
        }

        [TestMethod]
        public void ExistsFile()
        {
            var context = ContextFactory.GetMemoryContext("File Exists Test");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var root = new Folder
            {
                Id = 2,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            var file = new TxtFile
            {
                Id = 3,
                Name = "Folder",
                ParentFolder = root,
            };
            folderRepository.Insert(root);
            fileRepository.Insert(file);
            folderRepository.Save();

            var exists = fileRepository.Exists(file);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void TxtDbSet()
        {
            var context = ContextFactory.GetMemoryContext("Txt Test");
            var repository = new FileRepository(context);
            var file = new TxtFile
            {
                Id = 3,
                Name = "Folder",
                Content = ""
            };
            repository.Insert(file);
            repository.Save();
            Assert.IsTrue(context.Txts.Contains(file));
        }
    }
}