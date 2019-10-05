using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.DataAccess.Exceptions;
using TwoDrive.Domain;

namespace TwoDrive.DataAccess.Tests
{
    [TestClass]
    public class WriterRepositoryTests
    {
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
        [ExpectedException(typeof(DatabaseActionFailureException))]
        public void DeleteNonExistantWriter()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext15");
            var repository = new WriterRepository(memoryDb);
            repository.Delete(1);
            repository.Save();

        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseActionFailureException))]
        public void UpdateNonExistantWriter()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext16");
            var repository = new WriterRepository(memoryDb);
            repository.Update(null);
            repository.Save();

        }

        [TestMethod]
        [ExpectedException(typeof(DatabaseActionFailureException))]
        public void AddNonExistantWriter()
        {
            var memoryDb = ContextFactory.GetMemoryContext("TwoDriveContext17");
            var repository = new WriterRepository(memoryDb);
            repository.Insert(null);
            repository.Save();

        }
    }
}