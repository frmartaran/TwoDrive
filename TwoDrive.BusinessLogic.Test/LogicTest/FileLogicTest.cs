using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Test.LogicTest
{
    [TestClass]
    public class FileLogicTest
    {
        private TxtFile file;

        private Folder root;

        [TestInitialize]
        public void SetUp()
        {
            root = new Folder
            {
                Name = "Root"
            };
            file = new TxtFile
            {
                Id = 1,
                Content = "TestFile",
                CreationDate = DateTime.Now,
                DateModified = DateTime.Now,
                ParentFolder = root
            };
        }

        [TestMethod]
        public void CreateFile()
        {
            var mockRepository = new Mock<IRepository<File>>(MockBehavior.Strict);
            mockRepository
            .Setup(m => m.Insert(It.IsAny<File>()));

            var logic = new FileLogic(mockRepository.Object);
            logic.Create(file);

            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void CreateFileLogicCheckState()
        {
            var context = ContextFactory.GetMemoryContext("Create Test");
            var fileRepository = new FileRepository(context);
            var fileLogic = new FileLogic(fileRepository);
            fileLogic.Create(file);
            var fileInsertedInDB = fileLogic.Get(1);
            Assert.AreEqual(1, fileInsertedInDB.Id);
        }

        [TestMethod]
        public void DeleteFile()
        {
            var mockRepository = new Mock<IRepository<File>>(MockBehavior.Strict);
            mockRepository
            .Setup(m => m.Delete(It.IsAny<int>()));
            mockRepository.Setup(m => m.Save());

            var logic = new FileLogic(mockRepository.Object);
            logic.Delete(file.Id);

            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void DeleteOneFile()
        {
            var context = ContextFactory.GetMemoryContext("Delete Test");
            var fileRepository = new FileRepository(context);
            var folderRepository = new FolderRepository(context);

            folderRepository.Insert(root);
            fileRepository.Insert(file);

            var logic = new FileLogic(fileRepository);
            logic.Delete(file.Id);

            Assert.IsNull(fileRepository.Get(file.Id));
        }
    }
}