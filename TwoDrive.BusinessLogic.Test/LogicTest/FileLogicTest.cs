using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.DataAccess;
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
            var mockRepository = new Mock<Repository<TxtFile>>();
            mockRepository
            .Setup(m => m.Insert(It.IsAny<TxtFile>()));

            var logic = new FileLogic(mockRepository.Object);
            logic.Create(file);

            mockRepository.VerifyAll();

        }
    }
}