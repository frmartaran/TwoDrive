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
        [TestCleanup]
        public void CleanUp()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext");
            memoryDb.Set<Writer>().RemoveRange(memoryDb.Set<Writer>());
            memoryDb.Set<Element>().RemoveRange(memoryDb.Set<Element>());
            memoryDb.SaveChanges();
        }

        [TestMethod]
        public void GetInMemoryDatabase()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext");
            Assert.IsNotNull(memoryDb);
        }

        [TestMethod]
        public void AddAWriter()
        {

            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext");
            var writer = new Writer
            {
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
            Assert.AreEqual(writer.Id, writerInDb.Id);

        }

        [TestMethod]
        public void AddAFolder()
        {

            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext");
            var folder = new Folder
            {
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

            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext");
            var folder = new TxtFile
            {
                Name = "File",

            };

            var repository = new CrudOperations<Element>(memoryDb);
            repository.Create(folder);
            repository.Save();

            var writerInDb = memoryDb.Set<Element>().FirstOrDefault();
            Assert.AreEqual(folder.Id, writerInDb.Id);

        }


        [TestMethod]
        public void FindWriter()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext");
            var writer = new Writer
            {
                Token = Guid.NewGuid(),
                UserName = "WRiter",
                Password = "Pass",
                Claims = new List<Claim>(),
                Friends = new List<Writer>()
            };

            var repository = new CrudOperations<Writer>(memoryDb);
            repository.Create(writer);
            repository.Save();

            var writerFound = repository.Read(writer.Id);
            Assert.AreEqual(writer.Id, writerFound.Id);
        }

        [TestMethod]
        public void FindTxtFile()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext");
            var folder = new TxtFile
            {
                Name = "File",

            };



        }

    }
}