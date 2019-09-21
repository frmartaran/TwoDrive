using System;
using System.Collections.Generic;
using System.Linq;
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
                Token = Guid.NewGuid(),
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
            var repository = new Repository<Writer>(context);
            var validator = new WriterValidator();
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
            var repository = new Repository<Writer>(context);
            var validator = new WriterValidator();
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
            var repository = new Repository<Writer>(context);
            var validator = new WriterValidator();
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
            var context = ContextFactory.GetMemoryContext("Some Context");
            var repository = new Repository<Writer>(context);
            var validator = new WriterValidator();
            var logic = new WriterLogic(repository, validator);

            var newFolder = new Folder
            {
                Id = 1,
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
            var currentWriter = logic.Get(1);
            var newClaim = currentWriter.Claims.Where(c => c == claim).FirstOrDefault();
            Assert.AreEqual(claim, newClaim);
        }

    }
}