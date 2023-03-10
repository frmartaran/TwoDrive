using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.Domain;
using TwoDrive.BusinessLogic.Exceptions;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class FileLogicTest
    {
        private TxtFile file;

        private Folder root;

        [TestInitialize]
        public void SetUp()
        {
            var writer = new Writer();
            root = new Folder
            {
                Id = 1,
                Name = "Root",
                FolderChildren = new List<Element>()
            };
            file = new TxtFile
            {
                Id = 2,
                Content = "TestFile",
                Name = "FileName",
                CreationDate = DateTime.Now,
                DateModified = DateTime.Now,
                ParentFolder = root,
                Owner = writer
            };
        }

        [TestMethod]
        public void CreateFile()
        {
            var mockRepository = new Mock<IFileRepository>(MockBehavior.Strict);
            var mockValidator = new Mock<IFolderValidator>(MockBehavior.Strict);
            mockRepository
            .Setup(m => m.Insert(It.IsAny<File>()));
            mockRepository
            .Setup(m => m.Save());
            mockValidator
            .Setup(m => m.IsValid(It.IsAny<File>()))
            .Returns(true);

            var logic = new FileLogic(mockRepository.Object, mockValidator.Object);
            logic.Create(file);

            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void CreateFileLogicCheckState()
        {
            var context = ContextFactory.GetMemoryContext("Create Test fileTests");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);

            folderRepository.Insert(root);
            folderRepository.Save();
            var fileValidator = new FileValidator();
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            fileLogic.Create(file);

            var fileInsertedInDB = fileLogic.Get(2);
            Assert.AreEqual(2, fileInsertedInDB.Id);
        }

        [TestMethod]
        public void GetFileLogic()
        {
            var mockFileRepository = new Mock<IFileRepository>(MockBehavior.Strict);
            mockFileRepository.Setup(m => m.Get(It.IsAny<int>()))
                        .Returns(file);
            var mockValidator = new Mock<IFolderValidator>();
            var logic = new FileLogic(mockFileRepository.Object, mockValidator.Object);
            var fileReturned = logic.Get(1);

            mockFileRepository.VerifyAll();
        }

        [TestMethod]
        public void GetFileLogicCheckState()
        {
            var context = ContextFactory.GetMemoryContext("Get fileTests");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var validator = new Mock<IFolderValidator>();
            folderRepository.Insert(root);
            folderRepository.Save();
            fileRepository.Insert(file);
            fileRepository.Save();

            var logic = new FileLogic(fileRepository, validator.Object);
            var fileInDb = logic.Get(2);

            Assert.AreEqual(file.Id, fileInDb.Id);
        }

        [TestMethod]
        public void GetAllFileLogic()
        {
            var mockFileRepository = new Mock<IFileRepository>(MockBehavior.Strict);
            mockFileRepository.Setup(m => m.GetAll())
                        .Returns(new List<File> { file });

            var validator = new Mock<IFolderValidator>();
            var logic = new FileLogic(mockFileRepository.Object, validator.Object);
            var allFilesReturned = logic.GetAll();

            mockFileRepository.VerifyAll();
        }

        [TestMethod]
        public void GetAllFileLogicCheckState()
        {
            var context = ContextFactory.GetMemoryContext("Get all test fileTests");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);

            folderRepository.Insert(root);
            folderRepository.Save();
            fileRepository.Insert(file);
            fileRepository.Save();

            var validator = new Mock<IFolderValidator>();
            var logic = new FileLogic(fileRepository, validator.Object);
            var allFilesReturned = logic.GetAll();
            var firstFileReturned = allFilesReturned.FirstOrDefault();

            Assert.AreEqual(1, allFilesReturned.Count);
            Assert.AreEqual(file.Id, firstFileReturned.Id);
        }

        [TestMethod]
        public void DeleteFile()
        {
            var mockRepository = new Mock<IFileRepository>(MockBehavior.Strict);
            mockRepository
            .Setup(m => m.Delete(It.IsAny<int>()));
            mockRepository.Setup(m => m.Save());

            var validator = new Mock<IFolderValidator>();
            var logic = new FileLogic(mockRepository.Object, validator.Object);
            logic.Delete(file.Id);

            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void DeleteOneFile()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test fileTests");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);

            folderRepository.Insert(root);
            fileRepository.Insert(file);

            var validator = new Mock<IFolderValidator>();
            var logic = new FileLogic(fileRepository, validator.Object);
            logic.Delete(file.Id);

            Assert.IsNull(fileRepository.Get(file.Id));
        }

        [TestMethod]
        public void UpdateFileLogic()
        {
            var mockFileRepository = new Mock<IFileRepository>(MockBehavior.Strict);
            var mockFileValidator = new Mock<IFolderValidator>(MockBehavior.Strict);

            mockFileRepository.Setup(m => m.Update(It.IsAny<File>()));
            mockFileRepository.Setup(m => m.Save());
            mockFileValidator.Setup(m => m.IsValid(It.IsAny<File>()))
            .Returns(true);

            var logic = new FileLogic(mockFileRepository.Object, mockFileValidator.Object);
            logic.Update(file);
            mockFileRepository.VerifyAll();
            mockFileValidator.VerifyAll();
        }

        [TestMethod]
        public void UpdateOneFile()
        {
            var context = ContextFactory.GetMemoryContext("Update Test fileTests");
            var fileRepository = new FileRepository(context);
            var fileValidator = new FileValidator();

            fileRepository.Insert(file);
            fileRepository.Save();

            var dateModified = file.DateModified;
            file.Content = "Modified content";

            var logic = new FileLogic(fileRepository, fileValidator);
            logic.Update(file);

            var fileInDb = fileRepository.Get(2);
            var txtFile = (TxtFile) fileInDb;

            Assert.AreEqual(typeof(TxtFile), fileInDb.GetType());
            Assert.AreNotEqual(dateModified, fileInDb.DateModified);
            Assert.AreEqual(file.Content, txtFile.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void DeleteNullFile()
        {
            var context = ContextFactory.GetMemoryContext("Delete null File");
            var repository = new FileRepository(context);

            var validator = new Mock<IFolderValidator>();
            var logic = new FileLogic(repository, validator.Object);
            logic.Delete(1);
        }
    }
}