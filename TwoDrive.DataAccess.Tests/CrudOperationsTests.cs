using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess.Tests
{
    [TestClass]
    public class CrudOperationsTests
    {
        private List<TxtFile> mockData;

        private CrudOperations<TxtFile> crudOperations;

        [TestInitialize]
        public void Initialize()
        {
            var mockSet = new Mock<DbSet<TxtFile>>();
            var mockContext = new Mock<TwoDriveDbContext>();
            mockData = new List<TxtFile>
            {
                new TxtFile
                {
                    Id = 1,
                    Content = "testCreation1"
                },
                new TxtFile
                {
                    Id = 2,
                    Content = "testCreation2"
                }
            };
            mockContext.Setup(m => m.Set<TxtFile>()).Returns(mockSet.Object);
            mockSet.Setup(m => m.Add(It.IsAny<TxtFile>()))
                .Returns((TxtFile t) => null)
                .Callback<TxtFile>((t) => mockData.Add(t));
            mockSet.Setup(m => m.Find(It.IsAny<int>()))
                .Returns<object[]>(t => mockData.Find(d => d.Id == (int)t[0]));
            mockSet.Setup(m => m.Attach(It.IsAny<TxtFile>()))
                .Returns((TxtFile t) => null)
                .Callback<TxtFile>((t) =>
                {
                    mockData.Where(txt => txt.Id != t.Id).ToList();
                    mockData.Add(t);
                });
            mockSet.Setup(m => m.Remove(It.IsAny<TxtFile>()))
                .Returns((TxtFile t) => null)
                .Callback<TxtFile>((t) =>
                {
                    mockData = mockData.Where(txt => txt.Id != t.Id).ToList();
                });
            crudOperations = new CrudOperations<TxtFile>(mockContext.Object);
        }

        [TestMethod]
        public void TestCreateObject()
        {
            var txtFile = new TxtFile
            {
                Id = 3,
                Content = "testCreation3"
            };
            crudOperations.Create(txtFile);
            crudOperations.Save();

            Assert.AreEqual(3, mockData[2].Id);
            Assert.AreEqual("testCreation3", mockData[2].Content);
        }

        [TestMethod]
        public void TestReadObject()
        {
            var fileResult = crudOperations.Read(2);

            Assert.AreEqual(2, fileResult.Id);
            Assert.AreEqual("testCreation2", fileResult.Content);
        }

        [TestMethod]
        public void TestUpdateObject()
        {
            var fileResult = crudOperations.Read(1);
            fileResult.Content = "testCreation1Updated";
            crudOperations.Update(fileResult);
            crudOperations.Save();
            fileResult = crudOperations.Read(1);

            Assert.AreEqual(1, fileResult.Id);
            Assert.AreEqual("testCreation1Updated", fileResult.Content);
        }

        [TestMethod]
        public void TestDeleteObject()
        {
            crudOperations.Delete(1);
            crudOperations.Save();
            var fileResult = crudOperations.Read(1);

            Assert.AreSame(null, fileResult);
        }

        [TestMethod]
        public void GetInMemoryDatabase()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext");
            Assert.IsNotNull(memoryDb);
        }
    }
}