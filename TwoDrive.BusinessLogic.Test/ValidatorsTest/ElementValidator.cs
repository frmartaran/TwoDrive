using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Test
{
    public class ElementValidatorTest
    {
        private Writer owner;

        [TestInitialize]
        public void SetUp()
        {
            owner = new Writer
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid(),
                UserName = "Owner"
            };
        }

        [TestMethod]
        public void ValidRootFolder()
        {

            var folder = new Folder
            {
                Name = "Root",
                Owner = owner,
                ParentFolder = null
            };

            var validator = new ElementValidator();
            var isValid = validator.IsValid(folder);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidFolder()
        {

            var root = new Folder
            {
                Name = "Root",
                Owner = owner,
                ParentFolder = null
            };

            var child = new Folder
            {
                Name = "Child",
                Owner = owner,
                ParentFolder = root
            };

            var validator = new ElementValidator();
            var isValid = validator.IsValid(child);

            Assert.IsTrue(isValid);
        }
    }
}