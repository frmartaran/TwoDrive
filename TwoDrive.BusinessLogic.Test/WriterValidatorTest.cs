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
        public void InvalidWriterNoToken()
        {
            var writer = new Writer{
                UserName = "Writer",
                Password = "A password",
                Friends = new List<Writer>(),
                Claims = new List<Claim>(),
            };

            var validator = new WriterValidator();
            bool isValid = validator.isValid(writer);

            Assert.AreEqual(false, isValid);
        }
    }
}
