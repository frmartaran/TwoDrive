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
        public void AddAWriter()
        {

            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext2");
            var writer = new Writer
            {
                Id = 1,
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };

            var repository = new WriterRepository(memoryDb);
            repository.Insert(writer);
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

            var repository = new FolderRepository(memoryDb);
            repository.Insert(folder);
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

            var repository = new FileRepository(memoryDb);
            repository.Insert(file);
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
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };

            var repository = new WriterRepository(memoryDb);
            repository.Insert(writer);
            repository.Save();

            var writerFound = repository.Get(1);
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
            var fileRepository = new FileRepository(memoryDb);
            fileRepository.Insert(file);
            fileRepository.Save();

            var fileInMemory = fileRepository.Get(1);
            Assert.AreEqual(file, fileInMemory);
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
            var repository = new FolderRepository(memoryDb);
            repository.Insert(folder);
            repository.Save();

            var folderInMemory = repository.Get(1);
            Assert.AreEqual(folder, folderInMemory);
        }

        [TestMethod]
        public void UpdateWriter()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext8");
            var writer = new Writer
            {
                Id = 1,
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var repository = new WriterRepository(memoryDb);
            repository.Insert(writer);
            repository.Save();

            Assert.AreEqual("WRiter", writer.UserName);

            writer.UserName = "Writer";
            repository.Update(writer);
            repository.Save();
            var foundWriter = repository.Get(1);

            Assert.AreEqual("Writer", foundWriter.UserName);
            Assert.AreEqual(writer, foundWriter);
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
                FolderChilden = new List<Element>()
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
        public void DeleteWriter()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext11");
            var writer = new Writer
            {
                Id = 1,
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var repository = new WriterRepository(memoryDb);
            repository.Insert(writer);
            repository.Save();

            var countBeforeDeleting = memoryDb.Set<Writer>().Count();
            Assert.AreEqual(1, countBeforeDeleting);

            repository.Delete(1);
            repository.Save();

            var countAfterDeleting = memoryDb.Set<Writer>().Count();
            Assert.AreEqual(0, countAfterDeleting);
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
        public void DeleteFolder()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext13");
            var repository = new FolderRepository(memoryDb);
            var folder = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChilden = new List<Element>()
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
        public void GetAllWriters()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext14");
            var writer = new Writer
            {
                Id = 1,
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var anotherWriter = new Writer
            {
                Id = 2,
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };
            var repository = new WriterRepository(memoryDb);
            repository.Insert(writer);
            repository.Insert(anotherWriter);
            repository.Save();

            var all = repository.GetAll();
            Assert.AreEqual(2, all.Count());
            Assert.IsTrue(all.Contains(writer));
            Assert.IsTrue(all.Contains(anotherWriter));
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
                FolderChilden = new List<Element>()
            };
            var child = new Folder
            {
                Id = 2,
                Name = "Child",
                ParentFolder = folder,
                FolderChilden = new List<Element>()
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
        public void GetAllFilesAndFolders()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext18");
            var repository = new Repository<Element>(memoryDb);
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

        [TestMethod]
        public void CreateModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 1");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            var modification = new Modification
            {
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();

            var modificationInDb = context.Modifications.ToList().Count;
            Assert.AreEqual(1, modificationInDb);
        }

        [TestMethod]
        public void UpdateModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 2");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            var modification = new Modification
            {
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();
            modification.type = ModificationType.Changed;
            repository.Update(modification);
            repository.Save();

            var modificationInDb = context.Modifications.FirstOrDefault();
            Assert.AreEqual(ModificationType.Changed, modificationInDb.type);
        }

        [TestMethod]
        public void DeleteModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 3");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            var modification = new Modification
            {
                Id = 1,
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();

            repository.Delete(1);
            repository.Save();

            var modificationInDb = context.Modifications.ToList().Count;
            Assert.AreEqual(0, modificationInDb);
        }

        [TestMethod]
        public void GetModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 4");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            var modification = new Modification
            {
                Id = 1,
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();

            var modificationInDb = repository.Get(1);
            Assert.AreEqual(modification, modificationInDb);
        }

        [TestMethod]
        public void GetAllModifications()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 5");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            var modification = new Modification
            {
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();

            var modificationInDb = repository.GetAll();
            Assert.AreEqual(1, modificationInDb.Count);
            Assert.IsTrue(modificationInDb.Contains(modification));
        }

        [TestMethod]
        public void ExistsModification()
        {
            var context = ContextFactory.GetMemoryContext("Modification Test 6");
            var folder = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            var modification = new Modification
            {
                ElementModified = folder,
                type = ModificationType.Added
            };
            var repository = new ModificationRepository(context);
            repository.Insert(modification);
            repository.Save();

            var exists = repository.Exists(modification);
            Assert.IsTrue(exists);
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
        public void ExistsFolder()
        {
            var context = ContextFactory.GetMemoryContext("Folder Exists Test");
            var repository = new FolderRepository(context);
            var root = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChilden = new List<Element>()
            };
            var folder = new Folder
            {
                Id = 5,
                Name = "Folder",
                ParentFolder = root,
                FolderChilden = new List<Element>()
            };
            repository.Insert(root);
            repository.Insert(folder);
            repository.Save();

            var exists = repository.Exists(folder);
            Assert.IsTrue(exists);
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
                FolderChilden = new List<Element>()
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

        
        [TestMethod]
        public void ElementDbSet()
        {
            var context = ContextFactory.GetMemoryContext("Element Test");
            var repository = new Repository<Element>(context);
            
            var root = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChilden = new List<Element>()
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