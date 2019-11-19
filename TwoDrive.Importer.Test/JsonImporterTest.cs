using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TwoDrive.Importer.Interface.Exceptions;
using TwoDrive.Importer.Interface.IFileManagement;
using TwoDrive.Importer.Parameters;

namespace TwoDrive.Importer.Test
{
    [TestClass]
    public class JsonImporterTest
    {
        private const string examplesRoot = "..\\..\\..\\Json Tree Examples";

        [TestMethod]
        public void SetAndGetExtraParameters()
        {
            var importer = new JsonImporter();
            var parameters = importer.ExtraParameters;
            Assert.IsNotNull(parameters);
        }

        [TestMethod]
        public void SuccessfullyLoadJsonFile()
        {
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\baseCase.json"
            };
            var importer = new JsonImporter();
            var jsonFile = importer.Load<string>(parameters);

            Assert.IsNotNull(jsonFile);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileNotFound()
        {
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\doc.json"
            };
            var importer = new JsonImporter();
            var jsonFile = importer.Load<string>(parameters);
        }

        [TestMethod]
        public void SaveAFolder()
        {
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\baseCase.json"
            };
            var importer = new JsonImporter();
            var folder = importer.Import<IFolder>(parameters);

            Assert.IsNotNull(folder);
            Assert.IsInstanceOfType(folder, typeof(IFolder));
        }

        [TestMethod]
        public void SaveAChild()
        {
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\baseCase.json"
            };
            var importer = new JsonImporter();
            var folder = importer.Import<IFolder>(parameters);
            var child = folder.FolderChildren.FirstOrDefault();

            Assert.IsNotNull(child);
            Assert.IsInstanceOfType(folder, typeof(IFolder));
            Assert.IsInstanceOfType(child, typeof(IFile));
        }

        [TestMethod]
        public void SaveAChildFolder()
        {
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\baseCase.json"
            };
            var importer = new JsonImporter();
            var folder = importer.Import<IFolder>(parameters);
            var child = folder.FolderChildren.OfType<IFolder>().FirstOrDefault();

            Assert.IsNotNull(child);
            Assert.IsInstanceOfType(child, typeof(IFolder));

        }

        [TestMethod]
        public void SaveATwoFilesAndOneFolder()
        {
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\baseCase.json"
            };
            var importer = new JsonImporter();
            var folder = importer.Import<IFolder>(parameters);
            var folderChild = folder.FolderChildren.OfType<IFolder>().FirstOrDefault();
            var files = folder.FolderChildren.OfType<IFile>().ToList();

            Assert.IsNotNull(folderChild);
            Assert.IsInstanceOfType(folderChild, typeof(IFolder));
            Assert.AreEqual(2, files.Count);

        }

        [TestMethod]
        public void SaveThreeLevelTree()
        {
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\Level3Tree.json"
            };
            var importer = new JsonImporter();
            var folder = importer.Import<IFolder>(parameters);
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
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\MissingType.json"
            };
            var importer = new JsonImporter();
            var folder = importer.Import<IFolder>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WrongTypeInJson()
        {
            var path = $"{examplesRoot}\\WrongType.json";
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\WrongType.json"
            };
            var importer = new JsonImporter();
            var folder = importer.Import<IFolder>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WrongDateFormatInJson()
        {
            var path = $"{examplesRoot}\\WrongDateFormat.json";
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\WrongDateFormat.json"
            };
            var importer = new JsonImporter();
            var folder = importer.Import<IFolder>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WrongJson()
        {
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\WrongJson.json"
            };
            var importer = new JsonImporter();
            var folder = importer.Import<IFolder>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WrongTypeForThatElement()
        {
            var parameters = new JsonParameter
            {
                Path = $"{examplesRoot}\\WrongTypeForThatElement.json"
            };
            var importer = new JsonImporter();
            var folder = importer.Import<IFolder>(parameters);
        }
    }
}
