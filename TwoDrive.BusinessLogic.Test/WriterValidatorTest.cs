using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic.Test
{
    [TestClass]
    public class WriterValidatorTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterNoToken()
        {
            var writer = new Writer
            {
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = new List<Claim>(),
            };

            var validator = new WriterValidator();
            bool isValid = validator.isValid(writer);

        }

        [TestMethod]
        public void ValidWriter()
        {
            var writer = new Writer
            {
                Token = Guid.NewGuid(),
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = new List<Claim>(),
            };

            var validator = new WriterValidator();
            bool isValid = validator.isValid(writer);

            Assert.AreEqual(true, isValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterNoUserName()
        {
            var writer = new Writer
            {
                Token = Guid.NewGuid(),
                UserName = "",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = new List<Claim>(),
            };

            var validator = new WriterValidator();
            bool isValid = validator.isValid(writer);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterHasNoPassword()
        {
            var writer = new Writer
            {
                Token = Guid.NewGuid(),
                UserName = "Writer",
                Password = "",
                Friends = new List<Writer>(),
                Claims = new List<Claim>(),
            };

            var validator = new WriterValidator();
            bool isValid = validator.isValid(writer);
        }

        [TestMethod]
        public void WriterHasFriend()
        {
            var writer = new Writer
            {
                Token = Guid.NewGuid(),
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = new List<Claim>(),
            };

            var friend = new Writer
            {
                Token = Guid.NewGuid(),
                UserName = "Frined",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = new List<Claim>(),
            };

            writer.Friends.Add(friend);
            var validator = new WriterValidator();
            bool isValid = validator.isValid(writer);

            Assert.IsTrue(isValid);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidWriterHasNoClaims()
        {
            var writer = new Writer
            {
                Token = Guid.NewGuid(),
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = new List<Claim>(),
            };

            var validator = new WriterValidator();
            bool isValid = validator.isValid(writer);
        }

    }
}
