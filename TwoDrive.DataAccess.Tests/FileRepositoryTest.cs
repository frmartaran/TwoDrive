
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.DataAccess.Interface.LogicInput;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess.Tests
{
    [TestClass]
    public class FileRepositoryTest
    {
        private Folder root1;

        private Folder root2;

        private TxtFile fileRoot1;

        private TxtFile fileRoot2;

        private TxtFile fileRoot3;

        private TxtFile fileRoot4;

        private TxtFile fileRoot5;

        private TxtFile fileRoot6;

        private Writer firstWriter;

        private Writer secondWriter;

        [TestInitialize]
        public void SetUp()
        {
            firstWriter = new Writer
            {
                Id = 6
            };

            secondWriter = new Writer
            {
                Id = 7
            };

            root1 = new Folder
            {
                Id = 2,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = firstWriter,
                OwnerId = firstWriter.Id
            };

            root2 = new Folder
            {
                Id = 3,
                Name = "Root",
                FolderChildren = new List<Element>(),
                Owner = secondWriter,
                OwnerId = secondWriter.Id
            };

            fileRoot1 = new TxtFile
            {
                Id = 4,
                Name = "File1",
                Content = "",
                ParentFolder = root1,
                ParentFolderId = root1.Id,
                DateModified = new DateTime(2019, 1, 1),
                CreationDate = new DateTime(2019, 1, 1),
                Owner = firstWriter,
                OwnerId = firstWriter.Id
            };

            fileRoot2 = new TxtFile
            {
                Id = 5,
                Name = "File2",
                Content = "",
                ParentFolder = root2,
                ParentFolderId = root2.Id,
                DateModified = new DateTime(2019, 1, 2),
                CreationDate = new DateTime(2019, 1, 2),
                Owner = secondWriter,
                OwnerId = secondWriter.Id
            };

            fileRoot3 = new TxtFile
            {
                Id = 6,
                Name = "File3",
                Content = "",
                ParentFolder = root1,
                ParentFolderId = root1.Id,
                DateModified = new DateTime(2019, 1, 1),
                CreationDate = new DateTime(2019, 1, 1)
            };

            fileRoot4 = new TxtFile
            {
                Id = 7,
                Name = "File3",
                Content = "",
                ParentFolder = root2,
                ParentFolderId = root2.Id,
                DateModified = new DateTime(2019, 1, 2),
                CreationDate = new DateTime(2019, 1, 1)
            };

            fileRoot5 = new TxtFile
            {
                Id = 8,
                Name = "File5",
                Content = "",
                ParentFolder = root1,
                ParentFolderId = root1.Id,
                DateModified = new DateTime(2019, 1, 1),
                CreationDate = new DateTime(2019, 1, 1)
            };

            fileRoot6 = new TxtFile
            {
                Id = 9,
                Name = "File5",
                Content = "",
                ParentFolder = root2,
                ParentFolderId = root2.Id,
                DateModified = new DateTime(2019, 1, 1),
                CreationDate = new DateTime(2019, 1, 2)
            };

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

        [TestMethod]
        public void GetAllIsAdmin()
        {
            var context = ContextFactory.GetMemoryContext("Get All Is Admin");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);
            folderRepository.Insert(root1);
            folderRepository.Insert(root2);
            fileRepository.Insert(fileRoot1);
            fileRepository.Insert(fileRoot2);
            folderRepository.Save();
            fileRepository.Save();

            var filter = new FileFilter();
            var result = fileRepository.GetAll(filter);

            Assert.IsNotNull(result.Where(f => f.Id == 4).FirstOrDefault());
            Assert.IsNotNull(result.Where(f => f.Id == 5).FirstOrDefault());
        }

        [TestMethod]
        public void GetAllIsNotAdmin()
        {
            var context = ContextFactory.GetMemoryContext("Get All Is Not Admin");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);
            folderRepository.Insert(root1);
            folderRepository.Insert(root2);
            fileRepository.Insert(fileRoot1);
            fileRepository.Insert(fileRoot2);
            folderRepository.Save();
            fileRepository.Save();

            var filter = new FileFilter
            {
                Id = 6
            };
            var result = fileRepository.GetAll(filter);

            Assert.IsNotNull(result.Where(f => f.Id == 4).FirstOrDefault());
        }

        [TestMethod]
        public void GetAllOrderByDescendingName()
        {
            var context = ContextFactory.GetMemoryContext("Get All Order By Descending Name");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);
            folderRepository.Insert(root1);
            folderRepository.Insert(root2);
            fileRepository.Insert(fileRoot1);
            fileRepository.Insert(fileRoot2);
            folderRepository.Save();
            fileRepository.Save();

            var filter = new FileFilter 
            {
                IsOrderDescending = true,
                IsOrderByName = true
            };
            var result = fileRepository.GetAll(filter);

            Assert.AreEqual(result.FirstOrDefault().Name, "File2");
        }

        [TestMethod]
        public void GetAllOrderByDescendingDateModified()
        {
            var context = ContextFactory.GetMemoryContext("Get All Order By Descending Date Modified");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);
            folderRepository.Insert(root1);
            folderRepository.Insert(root2);
            fileRepository.Insert(fileRoot1);
            fileRepository.Insert(fileRoot2);
            folderRepository.Save();
            fileRepository.Save();

            var filter = new FileFilter
            {
                IsOrderDescending = true,
                IsOrderByModificationDate = true
            };
            var result = fileRepository.GetAll(filter);

            Assert.AreEqual(result.FirstOrDefault().DateModified.Year, 2019);
            Assert.AreEqual(result.FirstOrDefault().DateModified.Month, 1);
            Assert.AreEqual(result.FirstOrDefault().DateModified.Day, 2);
        }

        [TestMethod]
        public void GetAllOrderByDescendingCreationDate()
        {
            var context = ContextFactory.GetMemoryContext("Get All Order By Descending Creation Date");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);
            folderRepository.Insert(root1);
            folderRepository.Insert(root2);
            fileRepository.Insert(fileRoot1);
            fileRepository.Insert(fileRoot2);
            folderRepository.Save();
            fileRepository.Save();

            var filter = new FileFilter
            {
                IsOrderDescending = true,
                IsOrderByCreationDate = true
            };
            var result = fileRepository.GetAll(filter);

            Assert.AreEqual(result.FirstOrDefault().CreationDate.Year, 2019);
            Assert.AreEqual(result.FirstOrDefault().CreationDate.Month, 1);
            Assert.AreEqual(result.FirstOrDefault().CreationDate.Day, 2);
        }

        [TestMethod]
        public void GetAllOrderByName()
        {
            var context = ContextFactory.GetMemoryContext("Get All Order By Name");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);
            folderRepository.Insert(root1);
            folderRepository.Insert(root2);
            fileRepository.Insert(fileRoot1);
            fileRepository.Insert(fileRoot2);
            folderRepository.Save();
            fileRepository.Save();

            var filter = new FileFilter
            {
                IsOrderByName = true
            };
            var result = fileRepository.GetAll(filter);

            Assert.AreEqual(result.FirstOrDefault().Name, "File1");
        }

        [TestMethod]
        public void GetAllOrderByDateModified()
        {
            var context = ContextFactory.GetMemoryContext("Get All Order By Date Modified");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);
            folderRepository.Insert(root1);
            folderRepository.Insert(root2);
            fileRepository.Insert(fileRoot1);
            fileRepository.Insert(fileRoot2);
            folderRepository.Save();
            fileRepository.Save();

            var filter = new FileFilter
            {
                IsOrderByModificationDate = true
            };
            var result = fileRepository.GetAll(filter);

            Assert.AreEqual(result.FirstOrDefault().DateModified.Year, 2019);
            Assert.AreEqual(result.FirstOrDefault().DateModified.Month, 1);
            Assert.AreEqual(result.FirstOrDefault().DateModified.Day, 1);
        }

        [TestMethod]
        public void GetAllOrderByCreationDate()
        {
            var context = ContextFactory.GetMemoryContext("Get All Order By Creation Date");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);
            folderRepository.Insert(root1);
            folderRepository.Insert(root2);
            fileRepository.Insert(fileRoot1);
            fileRepository.Insert(fileRoot2);
            folderRepository.Save();
            fileRepository.Save();

            var filter = new FileFilter
            {
                IsOrderByCreationDate = true
            };
            var result = fileRepository.GetAll(filter);

            Assert.AreEqual(result.FirstOrDefault().DateModified.Year, 2019);
            Assert.AreEqual(result.FirstOrDefault().DateModified.Month, 1);
            Assert.AreEqual(result.FirstOrDefault().DateModified.Day, 1);
        }

        [TestMethod]
        public void GetAllOrderByDescendingAllParameters()
        {
            var context = ContextFactory.GetMemoryContext("Get All Order By Descending All Parameters");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);
            folderRepository.Insert(root1);
            folderRepository.Insert(root2);
            fileRepository.Insert(fileRoot1);
            fileRepository.Insert(fileRoot2);
            fileRepository.Insert(fileRoot3);
            fileRepository.Insert(fileRoot4);
            fileRepository.Insert(fileRoot5);
            fileRepository.Insert(fileRoot6);
            folderRepository.Save();
            fileRepository.Save();

            var filter = new FileFilter
            {
                IsOrderDescending = true,
                IsOrderByCreationDate = true,
                IsOrderByModificationDate = true,
                IsOrderByName = true
            };
            var result = fileRepository.GetAll(filter)
                .ToList();

            Assert.AreEqual(9, result[0].Id);
            Assert.AreEqual(8, result[1].Id);
            Assert.AreEqual(7, result[2].Id);
            Assert.AreEqual(6, result[3].Id);
            Assert.AreEqual(5, result[4].Id);
            Assert.AreEqual(4, result[5].Id);
        }

        [TestMethod]
        public void GetAllOrderByAllParameters()
        {
            var context = ContextFactory.GetMemoryContext("Get All Order By All Parameters");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);
            folderRepository.Insert(root1);
            folderRepository.Insert(root2);
            fileRepository.Insert(fileRoot1);
            fileRepository.Insert(fileRoot2);
            fileRepository.Insert(fileRoot3);
            fileRepository.Insert(fileRoot4);
            fileRepository.Insert(fileRoot5);
            fileRepository.Insert(fileRoot6);
            folderRepository.Save();
            fileRepository.Save();

            var filter = new FileFilter
            {
                IsOrderByCreationDate = true,
                IsOrderByModificationDate = true,
                IsOrderByName = true
            };
            var result = fileRepository.GetAll(filter)
                .ToList();

            Assert.AreEqual(4, result[0].Id);
            Assert.AreEqual(5, result[1].Id);
            Assert.AreEqual(6, result[2].Id);
            Assert.AreEqual(7, result[3].Id);
            Assert.AreEqual(8, result[4].Id);
            Assert.AreEqual(9, result[5].Id);
        }
    }
}