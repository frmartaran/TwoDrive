using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml;
using TwoDrive.Importer.Interface.IFileManagement;
using TwoDrive.Importer.Domain;
using TwoDrive.Importers;
using TwoDrive.Importer.Interface.Exceptions;

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
            var root = formatter.Import(path);
            Assert.IsNotNull(root);
            Assert.AreEqual(root.Name, ImporterConstants.Root);
            Assert.AreEqual(1, root.FolderChildren.Count);
            Assert.IsNull(root.ParentFolder);
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
            var root = formatter.Import(path);

            Assert.IsNotNull(root);
            Assert.AreEqual(1, root.FolderChildren.Count);
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
            var root = formatter.Import(path);

            var middleFolder = root.FolderChildren
                .FirstOrDefault() as IFolder;
            var lastFolder = middleFolder.FolderChildren
                .FirstOrDefault();
            Assert.AreEqual(1, root.FolderChildren.Count);
            Assert.AreEqual(1, middleFolder.FolderChildren.Count);
            Assert.AreEqual(middleFolder, lastFolder.ParentFolder);
        }

        [TestMethod]
        public void SaveTwoLevelOfFoldersWithFile()
        {
            var path = $@"{examplesRoot}\\Two Level Tree With File.xml";
            var formatter = new XMLImporter();
            var root = formatter.Import(path);

            var middleFolder = root.FolderChildren
                .FirstOrDefault() as IFolder;
            var lastFolder = middleFolder.FolderChildren
                .FirstOrDefault() as IFolder;
            var file = lastFolder.FolderChildren.
                OfType<IFile>()
                .FirstOrDefault();
            Assert.AreEqual(1, root.FolderChildren.Count);
            Assert.AreEqual(1, middleFolder.FolderChildren.Count);
            Assert.AreEqual(middleFolder, lastFolder.ParentFolder);
            Assert.IsNotNull(file);
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
            var root = formatter.Import(path);

            var filesCount = root.FolderChildren
                .Where(e => e is File)
                .ToList()
                .Count;

            Assert.AreEqual(2, root.FolderChildren.Count);
            Assert.AreEqual(1, filesCount);
        }

        [TestMethod]
        public void SaveSimpleTreeWithTwoTypeOfFile()
        {
            var path = $@"{examplesRoot}\\Two Types Of Files.xml";
            var formatter = new XMLImporter();
            var root = formatter.Import(path);

            var files = root.FolderChildren
                .OfType<File>()
                .ToList();
            var htmlFile = files
                .Where(t => t.Type == "html")
                .FirstOrDefault();
            Assert.AreEqual(3, root.FolderChildren.Count);
            Assert.AreEqual(2, files.Count);
            Assert.IsTrue(htmlFile.ShouldRender);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileWtihNoName()
        {
            var path = $@"{examplesRoot}\\File with no name.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileWtihNoType()
        {
            var path = $@"{examplesRoot}\\File with no type.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileWtihNoContent()
        {
            var path = $@"{examplesRoot}\\File with no content.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileWtihNoCreationDate()
        {
            var path = $@"{examplesRoot}\\File with no creation date.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileWtihNoDateModified()
        {
            var path = $@"{examplesRoot}\\File with no date modified.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }

        [TestMethod]
        public void FileWtihNoRenderDefined()
        {
            var path = $@"{examplesRoot}\\File with no render.xml";
            var formatter = new XMLImporter();
            var root = formatter.Import(path);
            var file = root.FolderChildren
                .OfType<File>()
                .FirstOrDefault();
            Assert.IsFalse(file.ShouldRender);
        }

        [TestMethod]
        public void FolderChildHasFile()
        {
            var path = $@"{examplesRoot}\\Child has file.xml";
            var formatter = new XMLImporter();
            var root = formatter.Import(path);
            var child = root.FolderChildren
                .OfType<Folder>()
                .FirstOrDefault();
            var file = child.FolderChildren
                .OfType<File>()
                .FirstOrDefault();

            Assert.IsNotNull(child);
            Assert.AreEqual(child, file.ParentFolder);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void NoRoot()
        {
            var path = $@"{examplesRoot}\\No root.xml";
            var formatter = new XMLImporter();
            var tree = formatter.Import(path);
        }


    }
}
