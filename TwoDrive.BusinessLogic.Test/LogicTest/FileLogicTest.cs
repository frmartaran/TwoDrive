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

        [TestInitialize]
        public void SetUp()
        {
            var root = new Folder
            {
                Name = "Root"
            };
            file = new TxtFile
            {
                Content = "TestFile",
                CreationDate = DateTime.Now,
                DateModified = DateTime.Now
            };
            root.FolderChilden = new List<Element>
            {
                file
            };
        }

        [TestMethod]
        public void CreateFile()
        {
            var mockRepository = new Mock<IRepository<File>>();
            mockRepository
            .Setup(m => m.Insert(It.IsAny<File>()));

            var logic = new FileLogic(mockRepository.Object);
            logic.Create(file);

            mockRepository.VerifyAll();
        }
    }
}