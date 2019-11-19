using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml;
using TwoDrive.Importer.Interface.IFileManagement;
using TwoDrive.Importer.Domain;
using TwoDrive.Importers;
using TwoDrive.Importer.Interface.Exceptions;
using TwoDrive.Importer.Interface;

namespace TwoDrive.Importer.Test
{
    [TestClass]
    public class XMLImporterTest
    {
        private const string examplesRoot = "..\\..\\..\\Xml Tree Examples";
        [TestMethod]
        public void SetAndGetExtraParameters()
        {
            var importer = new XMLImporter();
            var parameters = importer.ExtraParameters;
            Assert.IsNotNull(parameters);
        }

        [TestMethod]
        public void SuccessfullyLoadFile()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\Single Folder.xml"
            };
            var formatter = new XMLImporter();
            var document = formatter.Load<XmlDocument>(parameters);
            Assert.IsNotNull(document);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileNotFound()
        {
            var parameters = new ImportingParameters
            {
                Path = "path"
            };
            string path = $@"{examplesRoot}\\Single Folder.xml";
            var formatter = new XMLImporter();
            var document = formatter.Load<XmlDocument>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WrongXMLFile()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\Wrong File.xml"
            };

            var formatter = new XMLImporter();
            var document = formatter.Load<XmlDocument>(parameters);
        }

        [TestMethod]
        public void SaveRoot()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\Single Folder.xml"
            };
            var formatter = new XMLImporter();
            var root = formatter.Import<IFolder>(parameters);
            Assert.IsNotNull(root);
            Assert.AreEqual(root.Name, ImporterConstants.Root);
            Assert.AreEqual(1, root.FolderChildren.Count);
            Assert.IsNull(root.ParentFolder);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WithoutCreationDate()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\WithoutCreationDate.xml"
            };
            var formatter = new XMLImporter();
            var root = formatter.Import<IFolder>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WithoutDateModified()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\WithoutDateModified.xml"
            };
            var formatter = new XMLImporter();
            var tree = formatter.Import<IFolder>(parameters);
        }

        [TestMethod]
        public void SaveTwoFolders()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\Single Folder.xml"
            };
            var formatter = new XMLImporter();
            var root = formatter.Import<IFolder>(parameters);

            Assert.IsNotNull(root);
            Assert.AreEqual(1, root.FolderChildren.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void InvalidCreationDate()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\InvalidCreationDate.xml"
            };
            var formatter = new XMLImporter();
            var tree = formatter.Import<IFolder>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void InvalidDateModified()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\InvalidDateModified.xml"
            };
            var formatter = new XMLImporter();
            var tree = formatter.Import<IFolder>(parameters);
        }

        [TestMethod]
        public void SaveTwoLevelOfFolders()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\Two Level Tree.xml"
            };
            var formatter = new XMLImporter();
            var root = formatter.Import<IFolder>(parameters);

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
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\Two Level Tree With File.xml"
            };
            var formatter = new XMLImporter();
            var root = formatter.Import<IFolder>(parameters);

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
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\NoName.xml"
            };
            var formatter = new XMLImporter();
            var tree = formatter.Import<IFolder>(parameters);
        }

        [TestMethod]
        public void SaveSimpleTreeWithFile()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\Simple Tree With File.xml"
            };
            var formatter = new XMLImporter();
            var root = formatter.Import<IFolder>(parameters);

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
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\Two Types Of Files.xml"
            };
            var formatter = new XMLImporter();
            var root = formatter.Import<IFolder>(parameters);

            var files = root.FolderChildren
                .OfType<File>()
                .ToList();
            var htmlFile = files
                .Where(t => t.Type == "HTML")
                .FirstOrDefault();
            Assert.AreEqual(3, root.FolderChildren.Count);
            Assert.AreEqual(2, files.Count);
            Assert.IsTrue(htmlFile.ShouldRender);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileWtihNoName()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\File with no name.xml"
            };
            var formatter = new XMLImporter();
            var tree = formatter.Import<IFolder>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileWtihNoType()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\File with no type.xml"
            };
            var formatter = new XMLImporter();
            var tree = formatter.Import<IFolder>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileWtihNoContent()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\File with no content.xml"
            };
            var formatter = new XMLImporter();
            var tree = formatter.Import<IFolder>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileWtihNoCreationDate()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\File with no creation date.xml"
            };
            var formatter = new XMLImporter();
            var tree = formatter.Import<IFolder>(parameters);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileWtihNoDateModified()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\File with no date modified.xml"
            };
            var formatter = new XMLImporter();
            var tree = formatter.Import<IFolder>(parameters);
        }

        [TestMethod]
        public void FileWtihNoRenderDefined()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\File with no render.xml"
            };
            var formatter = new XMLImporter();
            var root = formatter.Import<IFolder>(parameters);
            var file = root.FolderChildren
                .OfType<File>()
                .FirstOrDefault();
            Assert.IsFalse(file.ShouldRender);
        }

        [TestMethod]
        public void FolderChildHasFile()
        {
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\Child has file.xml"
            };
            var formatter = new XMLImporter();
            var root = formatter.Import<IFolder>(parameters);
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
            var parameters = new ImportingParameters
            {
                Path = $@"{examplesRoot}\\No root.xml"
            };
            var formatter = new XMLImporter();
            var tree = formatter.Import<IFolder>(parameters);
        }


    }
}
