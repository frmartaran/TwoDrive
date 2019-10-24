using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
