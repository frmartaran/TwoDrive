using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var path = $"{examplesRoot}\\baseCase.json";
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
            var path = $"{examplesRoot}\\baseCase.json";
            var importer = new JsonImporter();
            var folder = importer.Import(path);

            Assert.IsNotNull(folder);
            Assert.IsInstanceOfType(folder, typeof(IFolder));
        }

        [TestMethod]
        public void SaveAChild()
        {
            var path = $"{examplesRoot}\\baseCase.json";
            var importer = new JsonImporter();
            var folder = importer.Import(path);
            var child = folder.FolderChildren.FirstOrDefault();

            Assert.IsNotNull(child);
            Assert.IsInstanceOfType(folder, typeof(IFolder));
            Assert.IsInstanceOfType(child, typeof(IFile));
        }

        [TestMethod]
        public void SaveAChildFolder()
        {
            var path = $"{examplesRoot}\\baseCase.json";
            var importer = new JsonImporter();
            var folder = importer.Import(path);
            var child = folder.FolderChildren.OfType<IFolder>().FirstOrDefault();

            Assert.IsNotNull(child);
            Assert.IsInstanceOfType(child, typeof(IFolder));

        }

        [TestMethod]
        public void SaveATwoFilesAndOneFolder()
        {
            var path = $"{examplesRoot}\\baseCase.json";
            var importer = new JsonImporter();
            var folder = importer.Import(path);
            var folderChild = folder.FolderChildren.OfType<IFolder>().FirstOrDefault();
            var files = folder.FolderChildren.OfType<IFile>().ToList();

            Assert.IsNotNull(folderChild);
            Assert.IsInstanceOfType(folderChild, typeof(IFolder));
            Assert.AreEqual(2, files.Count);

        }

        [TestMethod]
        public void SaveThreeLevelTree()
        {
            var path = $"{examplesRoot}\\Level3Tree.json";
            var importer = new JsonImporter();
            var folder = importer.Import(path);
            var folderChild = folder.FolderChildren.OfType<IFolder>().FirstOrDefault();
            var folderGrandson = folder.FolderChildren.OfType<IFolder>().FirstOrDefault(); 

            Assert.IsNotNull(folder);
            Assert.IsNotNull(folderChild);
            Assert.IsNotNull(folderGrandson);
            Assert.AreEqual(2, folderGrandson.FolderChildren.Count);

        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void MissingTypeInJson()
        {
            var path = $"{examplesRoot}\\MissingType.json";
            var importer = new JsonImporter();
            var folder = importer.Import(path);
        }
    }
}
