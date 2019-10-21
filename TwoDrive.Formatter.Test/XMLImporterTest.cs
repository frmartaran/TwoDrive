using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TwoDrive.Importer.MockDomain;
using TwoDrive.Importers;
using TwoDrive.Importers.Exceptions;

namespace TwoDrive.Importer.Test
{
    [TestClass]
    public class XMLImporterTest
    {
        private const string examplesRoot = "..\\..\\..\\Xml Tree Examples";

        [TestMethod]
        public void SuccessfullyLoadFile()
        {
            string path = $@"{examplesRoot}\\Single Folder.xml";
            var formatter = new XMLImporter();
            var document = formatter.Load<XmlDocument>(path);
            Assert.IsNotNull(document);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileNotFound()
        {
            var path = "path";
            var formatter = new XMLImporter();
            var document = formatter.Load<XmlDocument>(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WrongXMLFile()
        {
            var path = $@"{examplesRoot}\\Wrong File.xml";
            var formatter = new XMLImporter();
            var document = formatter.Load<XmlDocument>(path);
        }

        [TestMethod]
        public void SaveRoot()
        {
            var path = $@"{examplesRoot}\\Single Folder.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
            var root = tree.FirstOrDefault();
            Assert.IsNotNull(tree);
            Assert.AreEqual(root.Name, ImporterConstants.Root);
            Assert.AreEqual(2, tree.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WithoutCreationDate()
        {
            var path = $@"{examplesRoot}\\WithoutCreationDate.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WithoutDateModified()
        {
            var path = $@"{examplesRoot}\\WithoutDateModified.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }

        [TestMethod]
        public void SaveTwoFolders()
        {
            var path = $@"{examplesRoot}\\Single Folder.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);

            var foldersCount = tree.Count;
            Assert.AreEqual(2, foldersCount);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void InvalidCreationDate()
        {
            var path = $@"{examplesRoot}\\InvalidCreationDate.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void InvalidDateModified()
        {
            var path = $@"{examplesRoot}\\InvalidDateModified.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }

        [TestMethod]
        public void SaveTwoLevelOfFolders()
        {
            var path = $@"{examplesRoot}\\Two Level Tree.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);

            var foldersCount = tree.Count;
            var middleFolder = tree.ElementAt(1);
            var lastFolder = tree.ElementAt(2);
            Assert.AreEqual(3, foldersCount);
            Assert.AreEqual(middleFolder, lastFolder.ParentFolder);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FolderWithNoName()
        {
            var path = $@"{examplesRoot}\\NoName.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }

        [TestMethod]
        public void SaveSimpleTreeWithFile()
        {
            var path = $@"{examplesRoot}\\Simple Tree With File.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);

            var foldersCount = tree.Count;
            var root = tree.FirstOrDefault();
            var filesCount = root.FolderChildren
                .Where(e => e is MockFile)
                .ToList()
                .Count;
            Assert.AreEqual(2, foldersCount);
            Assert.AreEqual(1, filesCount);
        }

        [TestMethod]
        public void SaveSimpleTreeWithTwoTypeOfFile()
        {
            var path = $@"{examplesRoot}\\Two Types Of Files.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);

            var foldersCount = tree.Count;
            var root = tree.FirstOrDefault();
            var files = root.FolderChildren
                .OfType<MockFile>()
                .ToList();

            Assert.AreEqual(2, foldersCount);
            Assert.AreEqual(2, files.Count);
        }

    }
}
