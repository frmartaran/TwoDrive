using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.DataAccess;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class WriterValidatorTest
    {
        private List<Claim> defaultClaims;
        private Folder root;
        private WriterRepository repository;

        [TestInitialize]
        public void SetUp()
        {
            var context = ContextFactory.GetMemoryContext("TwoDriveContext");
            repository = new WriterRepository(context);

            root = new Folder
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
            defaultClaims = new List<Claim>{
                write,
                read,
                share
            };
        }

        [TestMethod]
        public void ValidWriter()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };

            root.Owner = writer;
            var validator = new WriterValidator(repository);
            bool isValid = validator.IsValid(writer);

            Assert.AreEqual(true, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterNoUserName()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };

            root.Owner = writer;
            var validator = new WriterValidator(repository);
            bool isValid = validator.IsValid(writer);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterHasNoPassword()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };

            root.Owner = writer;
            var validator = new WriterValidator(repository);
            bool isValid = validator.IsValid(writer);
        }

        [TestMethod]
        public void WriterHasFriend()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };

            var friend = new Writer
            {
                Id = 1,
                UserName = "Frined",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = new List<Claim>(),
            };

            root.Owner = writer;
            writer.Friends.Add(friend);
            var validator = new WriterValidator(repository);
            bool isValid = validator.IsValid(writer);

            Assert.IsTrue(isValid);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterHasNoClaims()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = new List<Claim>(),
            };

            root.Owner = writer;
            var validator = new WriterValidator(repository);
            bool isValid = validator.IsValid(writer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterClaimsListIsNull()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = null,
            };


            root.Owner = writer;
            var validator = new WriterValidator(repository);
            bool isValid = validator.IsValid(writer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterHasDeleteClaim()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };

            root.Owner = writer;
            var delete = new Claim
            {
                Element = root,
                Type = ClaimType.Delete
            };

            root.Owner = writer;
            writer.Claims.Add(delete);

            var validator = new WriterValidator(repository);
            bool isValid = validator.IsValid(writer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterMissingReadClaim()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };

            root.Owner = writer;
            var read = writer.Claims.FirstOrDefault(c => c.Type == ClaimType.Read);
            writer.Claims.Remove(read);

            var validator = new WriterValidator(repository);
            bool isValid = validator.IsValid(writer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterMissingWriteClaim()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };

            root.Owner = writer;
            var write = writer.Claims.FirstOrDefault(c => c.Type == ClaimType.Write);
            writer.Claims.Remove(write);

            var validator = new WriterValidator(repository);
            bool isValid = validator.IsValid(writer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterMissingShareClaim()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };

            root.Owner = writer;
            var share = writer.Claims.FirstOrDefault(c => c.Type == ClaimType.Share);
            writer.Claims.Remove(share);

            var validator = new WriterValidator(repository);
            bool isValid = validator.IsValid(writer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterUsernameExists()
        {

            var context = ContextFactory.GetMemoryContext("Context");
            var repository = new WriterRepository(context);
            var name = "Writer";
            var writer = new Writer
            {
                Id = 1,
                UserName = name,
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = defaultClaims,
            };
            repository.Insert(writer);
            repository.Save();
            var folder = root;
            var anotherWriter = new Writer
            {
                Id = 2,
                UserName = name,
                Password = "Holus",
                Friends = new List<Writer>(),
            };
            folder.Owner = anotherWriter;
            var claims = defaultClaims;
            claims.ToList().ForEach(c => c.Element = folder);
            anotherWriter.Claims = claims;

            var validator = new WriterValidator(repository);
            validator.IsValid(anotherWriter);

        }

    }
}
