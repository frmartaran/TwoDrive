using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface.IFileManagement;
using TwoDrive.Importers.Exceptions;

namespace TwoDrive.Importer.Test
{
    [TestClass]
    public class JsonImporterTest
    {
        private const string examplesRoot = "..\\..\\..\\Json Tree Examples";

        [TestMethod]
        public void SuccessfullyLoadJsonFile()
        {
            var path = $"{examplesRoot}\\document.json";
            var importer = new JsonImporter();
            var jsonFile = importer.Load<string>(path);

            Assert.IsNotNull(jsonFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileNotFound()
        {
            var path = $"{examplesRoot}\\doc.json";
            var importer = new JsonImporter();
            var jsonFile = importer.Load<string>(path);
        }

        [TestMethod]
        public void SaveAFolder()
        {
            var path = $"{examplesRoot}\\document.json";
            var importer = new JsonImporter();
            var folder = importer.Import(path);

            Assert.IsNotNull(folder);
            Assert.IsInstanceOfType(folder, typeof(IFolder));
        }
    }
}
