using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class WriterLogicTest
    {
        private Writer writer;
        [TestInitialize]
        public void SetUp()
        {
            var root = new Folder
            {
                Name = "Root"
            };
            var read = new Claim
            {
                Element = root,
                Type = ClaimType.Read
            };
            var write = new Claim
            {
                Element = root,
                Type = ClaimType.Write
            };
            var share = new Claim
            {
                Element = root,
                Type = ClaimType.Share
            };
            var defaultClaims = new List<Claim>{
                write,
                read,
                share
            };
            writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };
            root.Owner = writer;
        }
        [TestMethod]
        public void CreateUser()
        {
            var mockRepository = new Mock<IRepository<Writer>>(MockBehavior.Strict);
            mockRepository
            .Setup(m => m.Insert(It.IsAny<Writer>()));
            mockRepository.Setup(m => m.Save());

            var mockValidator = new Mock<IValidator<Writer>>(MockBehavior.Strict);
            mockValidator
            .Setup(m => m.isValid(It.IsAny<Writer>()))
            .Returns(true);

            var logic = new WriterLogic(mockRepository.Object, mockValidator.Object);
            logic.Create(writer);

            mockRepository.VerifyAll();
            mockValidator.VerifyAll();

        }

        [TestMethod]
        public void CreateWriterCheckState()
        {
            var context = ContextFactory.GetMemoryContext("TwoDrive");
            var repository = new WriterRepository(context);
            var validator = new WriterValidator(repository);
            var logic = new WriterLogic(repository, validator);
            logic.Create(writer);
            var writersInDb = repository.Get(1);
            Assert.AreEqual(writer, writersInDb);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateInvalidWriterCheckState()
        {
            var context = ContextFactory.GetMemoryContext("TwoDrive");
            var repository = new WriterRepository(context);
            var validator = new WriterValidator(repository);
            var logic = new WriterLogic(repository, validator);
            writer.UserName = "";
            logic.Create(writer);
        }

        [TestMethod]
        public void GetWriter()
        {
            var mockRepository = new Mock<IRepository<Writer>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.Get(It.IsAny<int>()))
            .Returns(writer);
            var logic = new WriterLogic(mockRepository.Object);
            var writerGotten = logic.Get(1);

            mockRepository.VerifyAll();

        }

        [TestMethod]
        public void GetWriterCheckState()
        {
            var context = ContextFactory.GetMemoryContext("TwoDriveContext");
            context.Set<Writer>().Add(writer);
            var repository = new Repository<Writer>(context);
            var logic = new WriterLogic(repository);
            var writerInDb = logic.Get(1);

            Assert.AreEqual(writer, writerInDb);
        }

        [TestMethod]
        public void UpdateWriter()
        {
            var mockRepository = new Mock<IRepository<Writer>>();
            mockRepository.Setup(m => m.Update(It.IsAny<Writer>()));
            var mockValidator = new Mock<IValidator<Writer>>();
            mockValidator.Setup(m => m.isValid(It.IsAny<Writer>()))
            .Returns(true);

            var logic = new WriterLogic(mockRepository.Object, mockValidator.Object);
            writer.UserName = "Another Username";
            logic.Update(writer);

            mockRepository.VerifyAll();
            mockValidator.VerifyAll();

        }

        [TestMethod]
        public void UpdateWriterCheckState()
        {
            var context = ContextFactory.GetMemoryContext("Some Context");

            var repository = new WriterRepository(context);
            repository.Insert(writer);
            repository.Save();

            var validator = new WriterValidator(repository);
            var logic = new WriterLogic(repository, validator);

            Assert.AreEqual(writer.UserName, "Writer");
            var newName = "Another Username";
            writer.UserName = newName;
            logic.Update(writer);
            var currentWriter = logic.Get(1);
            Assert.AreEqual(currentWriter.UserName, newName);
        }

        [TestMethod]
        public void UpdateWriterAddFolder()
        {
            var context = ContextFactory.GetMemoryContext("Test");

            var repository = new WriterRepository(context);
            repository.Insert(writer);
            repository.Save();
            var validator = new WriterValidator(repository);
            var logic = new WriterLogic(repository, validator);

            var newFolder = new Folder
            {
                Id = 2,
                Name = "Folder",
                Owner = writer,
                FolderChilden = new List<Element>()
            };

            var claim = new Claim
            {
                Element = newFolder,
                Type = ClaimType.Read
            };

            writer.Claims.Add(claim);
            repository.Update(writer);
            var writerInDb = repository.Get(1);
            var newClaim = writerInDb.Claims;
            Assert.AreEqual(true, newClaim.Contains(claim));
        }

        [TestMethod]
        public void GetAll()
        {
            var writers = new List<Writer>();
            var mockRepository = new Mock<IRepository<Writer>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.GetAll())
            .Returns(writers);
            var logic = new WriterLogic(mockRepository.Object);
            var list = logic.GetAll();
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void GetAllWriters()
        {
            var context = ContextFactory.GetMemoryContext("A test context");
            var repository = new WriterRepository(context);
            repository.Insert(writer);
            repository.Save();
            var logic = new WriterLogic(repository);
            var allWriters = logic.GetAll();
            Assert.AreEqual(1, allWriters.Count());
            Assert.IsTrue(allWriters.Contains(writer));
        }

        [TestMethod]
        public void DeleteWriter()
        {
            var mockRepository = new Mock<IRepository<Writer>>(MockBehavior.Strict);
            mockRepository.Setup(m => m.Delete(It.IsAny<int>()));
            mockRepository.Setup(m => m.Save());
            var logic = new WriterLogic(mockRepository.Object);
            logic.Delete(writer);

            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void DeleteWriterCheckState()
        {
            var context = ContextFactory.GetMemoryContext("Delete Context");
            var repository = new WriterRepository(context);
            repository.Insert(writer);
            repository.Save();
            var allWritersInDb = repository.GetAll();

            Assert.IsTrue(allWritersInDb.Contains(writer));
            Assert.AreEqual(1, allWritersInDb.Count());

            var logic = new WriterLogic(repository);
            logic.Delete(writer);

            var currentWritersInDb = repository.GetAll(); 

            Assert.IsFalse(currentWritersInDb.Contains(writer));
            Assert.AreEqual(0, currentWritersInDb.Count());           
        }
    }
}