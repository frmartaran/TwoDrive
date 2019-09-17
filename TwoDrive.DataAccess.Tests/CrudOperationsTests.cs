using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Domain;
using System;
using TwoDrive.DataAccess.Interface;

namespace TwoDrive.DataAccess.Tests
{
    [TestClass]
    public class CrudOperationsTests
    {
        [TestMethod]
        public void GetInMemoryDatabase()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext1");
            Assert.IsNotNull(memoryDb);
        }

        [TestMethod]
        public void AddAWriter()
        {

            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext2");
            var writer = new Writer
            {
                Id = 1,
                Token = Guid.NewGuid(),
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };

            var repository = new CrudOperations<Writer>(memoryDb);
            repository.Create(writer);
            repository.Save();

            var writerInDb = memoryDb.Set<Writer>().FirstOrDefault();
            Assert.AreEqual(writer, writerInDb);

        }

        [TestMethod]
        public void AddAFolder()
        {

            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext3");
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChilden = new List<Element>()
            };

            var repository = new CrudOperations<Element>(memoryDb);
            repository.Create(folder);
            repository.Save();

            var writerInDb = memoryDb.Set<Element>().FirstOrDefault();
            Assert.AreEqual(folder.Id, writerInDb.Id);

        }

        [TestMethod]
        public void AddAFile()
        {

            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext4");
            File file = new TxtFile
            {
                Id = 1,
                Name = "File",
            };

            var repository = new CrudOperations<Element>(memoryDb);
            repository.Create(file);
            repository.Save();

            var writerInDb = memoryDb.Set<Element>().FirstOrDefault();
            Assert.AreEqual(file.Id, writerInDb.Id);

        }


        [TestMethod]
        public void FindWriter()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext5");
            var writer = new Writer
            {
                Id = 1,
                Token = Guid.NewGuid(),
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };

            var repository = new CrudOperations<Writer>(memoryDb);
            repository.Create(writer);
            repository.Save();

            var writerFound = repository.Read(1);
            Assert.AreEqual(writer, writerFound);
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
            var repository = new CrudOperations<Element>(memoryDb);
            repository.Create(file);
            repository.Save();

            var folderInMemory = repository.Read(1);
            Assert.AreEqual(file, folderInMemory);

        }

        [TestMethod]
        public void FindFolder()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext7");
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            var repository = new CrudOperations<Element>(memoryDb);
            repository.Create(folder);
            repository.Save();

            var folderInMemory = repository.Read(1);
            Assert.AreEqual(folder, folderInMemory);

        }

        [TestMethod]
        public void UpdateWriter()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext8");
            var writer = new Writer
            {
                Id = 1,
                Token = Guid.NewGuid(),
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var repository = new CrudOperations<Writer>(memoryDb);
            repository.Create(writer);
            repository.Save();

            Assert.AreEqual("WRiter", writer.UserName);

            writer.UserName = "Writer";
            repository.Update(writer);
            repository.Save();
            var foundWriter = repository.Read(1);

            Assert.AreEqual("Writer", foundWriter.UserName);
            Assert.AreEqual(writer, foundWriter);

        }

        [TestMethod]
        public void UpdateFolder()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext9");
            var repository = new CrudOperations<Element>(memoryDb);
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            repository.Create(folder);
            repository.Save();

            Assert.AreEqual("Root", folder.Name);

            folder.Name = " root ";
            repository.Update(folder);
            repository.Save();
            var foundFolder = repository.Read(1);

            Assert.AreEqual(" root ", foundFolder.Name);
            Assert.AreEqual(folder, foundFolder);

        }

        [TestMethod]
        public void UpdateFile()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext10");
            var repository = new CrudOperations<Element>(memoryDb);
            var file = new TxtFile
            {
                Id = 1,
                Name = "File",

            };
            repository.Create(file);
            repository.Save();

            Assert.AreEqual("File", file.Name);

            file.Name = "TXT";
            repository.Update(file);
            repository.Save();
            var foundFile = repository.Read(1);

            Assert.AreEqual("TXT", foundFile.Name);
            Assert.AreEqual(file, foundFile);

        }

        [TestMethod]
        public void DeleteWriter()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext11");
            var writer = new Writer
            {
                Id = 1,
                Token = Guid.NewGuid(),
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var repository = new CrudOperations<Writer>(memoryDb);
            repository.Create(writer);
            repository.Save();

            var countBeforeDeleting = memoryDb.Set<Writer>().Count();
            Assert.AreEqual(1, countBeforeDeleting);

            repository.Delete(1);
            repository.Save();

            var countAfterDeleting = memoryDb.Set<Writer>().Count();
            Assert.AreEqual(0, countAfterDeleting);

        }

        [TestMethod]
        public void DeleteFolder()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext12");
            var repository = new CrudOperations<Element>(memoryDb);
            var file = new TxtFile
            {
                Id = 1,
                Name = "File",

            };
            repository.Create(file);
            repository.Save();

            var countBeforeDeleting = memoryDb.Set<Element>().Count();
            Assert.AreEqual(1, countBeforeDeleting);

            repository.Delete(1);
            repository.Save();

            var countAfterDeleting = memoryDb.Set<Element>().Count();
            Assert.AreEqual(0, countAfterDeleting);

        }

        [TestMethod]
        public void DeleteFile()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext13");
            var repository = new CrudOperations<Element>(memoryDb);
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            repository.Create(folder);
            repository.Save();

            var countBeforeDeleting = memoryDb.Set<Element>().Count();
            Assert.AreEqual(1, countBeforeDeleting);

            repository.Delete(1);
            repository.Save();

            var countAfterDeleting = memoryDb.Set<Element>().Count();
            Assert.AreEqual(0, countAfterDeleting);

        }

        [TestMethod]
        public void ReadAllWriters()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext14");
            var writer = new Writer
            {
                Id = 1,
                Token = Guid.NewGuid(),
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var anotherWriter = new Writer
            {
                Id = 2,
                Token = Guid.NewGuid(),
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var repository = new CrudOperations<Writer>(memoryDb);
            repository.Create(writer);
            repository.Create(anotherWriter);
            repository.Save();

            var all = repository.ReadAll();
            Assert.AreEqual(2, all.Count());
            Assert.IsTrue(all.Contains(writer));
            Assert.IsTrue(all.Contains(anotherWriter));

        }

        [TestMethod]
        public void ReadAllFolders()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext15");
            var repository = new CrudOperations<Folder>(memoryDb);
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            var child = new Folder
            {
                Id = 2,
                Name = "Child",
                ParentFolder = folder,
                FolderChilden = new List<Element>()
            };
            repository.Create(folder);
            repository.Create(child);
            repository.Save();

            var all = repository.ReadAll();
            Assert.AreEqual(2, all.Count());
            Assert.IsTrue(all.Contains(folder));
            Assert.IsTrue(all.Contains(child));
            Assert.IsTrue(all.All(f => f.GetType().Name == "Folder"));
        }

        [TestMethod]
        public void ReadAllFiles()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext16");
            var repository = new CrudOperations<File>(memoryDb);
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
            repository.Create(file);
            repository.Create(anotherFile);
            repository.Save();

            var all = repository.ReadAll();
            Assert.AreEqual(2, all.Count());
            Assert.IsTrue(all.Contains(file));
            Assert.IsTrue(all.Contains(anotherFile));
            Assert.IsTrue(all.All(f => f.GetType().Name == "TxtFile"));
        }

         [TestMethod]
        public void ReadAllFilesAndFolders()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext18");
            var repository = new CrudOperations<Element>(memoryDb);
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChilden = new List<Element>()
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
            repository.Create(file);
            repository.Create(anotherFile);
            repository.Create(folder);
            repository.Save();

            var all = repository.ReadAll();
            var fileCount = all.Where(e => e is TxtFile).Count();
            var folderCount = all.Where(e => e is Folder).Count();

            Assert.AreEqual(3, all.Count());
            Assert.IsTrue(all.Contains(file));
            Assert.IsTrue(all.Contains(anotherFile));
            Assert.IsTrue(all.Contains(folder));
            Assert.AreEqual(1, folderCount);
            Assert.AreEqual(2, fileCount);

        }

        
    }
}