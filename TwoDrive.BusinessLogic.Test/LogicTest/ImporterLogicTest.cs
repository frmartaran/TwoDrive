using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.Domain;
using TwoDrive.Importer;
using TwoDrive.Importers;

namespace TwoDrive.BusinessLogic.Test.LogicTest
{
    [TestClass]
    public class ImporterLogicTest
    {
        private Writer writer;

        [TestInitialize]
        public void SetUp()
        {
            writer = new Writer
            {
                Id = 1,
                UserName = "A Writer",
                Password = "1234",
                Role = Role.Writer,
                Claims = new List<CustomClaim>()
            };
        }

        [TestMethod]
        public void GetImporter()
        {
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockFileLogic = new Mock<IFileLogic>();
            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var dependencies = new ImporterLogicDependencies(mockFolderLogic.Object, 
                mockFileLogic.Object, mockWriterLogic.Object);
            var options = new ImportingOptions
            {
                FilePath = "",
                FileType = "XML",
                Owner = writer
            };
            var importerLogic = new ImporterLogic(options, dependencies);
            var importer = importerLogic.GetImporter();

            Assert.IsInstanceOfType(importer, typeof(XMLImporter));
        }

        [TestMethod]
        public void GetJsonImporter()
        {
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockFileLogic = new Mock<IFileLogic>();
            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var dependencies = new ImporterLogicDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object);
            var options = new ImportingOptions
            {
                FilePath = "",
                FileType = "JSON",
                Owner = writer
            };
            var importerLogic = new ImporterLogic(options, dependencies);
            var importer = importerLogic.GetImporter();

            Assert.IsInstanceOfType(importer, typeof(JsonImporter));
        }


    }
}
