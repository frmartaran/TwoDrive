using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Formatter.Interface;
using TwoDrive.Formatters;
using TwoDrive.Formatters.Exceptions;

namespace TwoDrive.Formatter.Test
{
    [TestClass]
    public class XMLFormatterTest
    {
        private const string examplesRoot = "..\\..\\..\\Xml Tree Examples";
        Writer writer;

        [TestInitialize]
        public void SetUp()
        {
            writer = new Writer
            {
                UserName = "Writer",
                Password = "123",
                Role = Role.Administrator,
                Claims = new List<CustomClaim>(),
                Friends = new List<Writer>()
            };
        }

        [TestMethod]
        public void SuccessfullyLoadFile()
        {
            string path = $@"{examplesRoot}\\Single Folder.xml";
            var mockFolderLogic = new Mock<IFolderLogic>();
            var formatter = new XMLFormatter(mockFolderLogic.Object);
            var document = formatter.Load<XmlDocument>(path);
            Assert.IsNotNull(document);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatterException))]
        public void FileNotFound()
        {
            var path = "path";
            var mockFolderLogic = new Mock<IFolderLogic>();
            var formatter = new XMLFormatter(mockFolderLogic.Object);
            var document = formatter.Load<XmlDocument>(path);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatterException))]
        public void WrongXMLFile()
        {
            var path = $@"{examplesRoot}\\Wrong File.xml";
            var mockFolderLogic = new Mock<IFolderLogic>();
            var formatter = new XMLFormatter(mockFolderLogic.Object);
            var document = formatter.Load<XmlDocument>(path);
        }

        [TestMethod]
        public void SaveRootMock()
        {
            var path = $@"{examplesRoot}\\Single Folder.xml";
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Create(It.IsAny<Folder>()));
            var formatter = new XMLFormatter(mockFolderLogic.Object)
            {
                WriterFor = writer
            };
            formatter.Import(path);

            mockFolderLogic.VerifyAll();
        }

        [TestMethod]
        public void SaveRoot()
        {
            var path = $@"{examplesRoot}\\Single Folder.xml";
            var context = ContextFactory.GetMemoryContext("Save Root");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository, 
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLFormatter(folderLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);

            var root = context.Folders.FirstOrDefault();
            var claimsForRoot = writer.Claims
                .Where(c => c.Element == root)
                .ToList()
                .Count;
            Assert.IsNotNull(root);
            Assert.AreEqual("Root", root.Name);
            Assert.AreEqual(3, claimsForRoot);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatterException))]
        public void WithoutCreationDate()
        {
            var path = $@"{examplesRoot}\\WithoutCreationDate.xml";
            var context = ContextFactory.GetMemoryContext("Without Creation Date");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLFormatter(folderLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatterException))]
        public void WithoutDateModified()
        {
            var path = $@"{examplesRoot}\\WithoutDateModified.xml";
            var context = ContextFactory.GetMemoryContext("Without Creation Date");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLFormatter(folderLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);
        }

        [TestMethod]
        public void SaveTwoFolders()
        {
            var path = $@"{examplesRoot}\\Single Folder.xml";
            var context = ContextFactory.GetMemoryContext("Save Two Folders");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLFormatter(folderLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);

            var foldersCount = context.Folders.ToList().Count;
            Assert.AreEqual(2, foldersCount);
            Assert.AreEqual(7, writer.Claims.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatterException))]
        public void InvalidCreationDate()
        {
            var path = $@"{examplesRoot}\\InvalidCreationDate.xml";
            var context = ContextFactory.GetMemoryContext("Without Creation Date");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLFormatter(folderLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatterException))]
        public void InvalidDateModified()
        {
            var path = $@"{examplesRoot}\\InvalidDateModified.xml";
            var context = ContextFactory.GetMemoryContext("Without Creation Date");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLFormatter(folderLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);
        }

        [TestMethod]
        public void SaveTwoLevelOfFolders()
        {
            var path = $@"{examplesRoot}\\Two Level Tree.xml";
            var context = ContextFactory.GetMemoryContext("Save Two Levels Of Folders");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLFormatter(folderLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);

            var foldersCount = context.Folders.ToList().Count;
            var middleFolder = context.Folders
                .ToList()
                .ElementAt(1);
            var lastFolder = context.Folders
                .ToList()
                .ElementAt(2);
            Assert.AreEqual(3, foldersCount);
            Assert.AreEqual(middleFolder, lastFolder.ParentFolder);
            Assert.AreEqual(11, writer.Claims.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatterException))]
        public void FolderWithNoName()
        {
            var path = $@"{examplesRoot}\\NoName.xml";
            var context = ContextFactory.GetMemoryContext("No Name");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLFormatter(folderLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);
        }

        [TestMethod]
        public void SaveSimpleTreeWithFile()
        {
            var path = $@"{examplesRoot}\\Simple Tree With File.xml";
            var context = ContextFactory.GetMemoryContext("Simple tree with file");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLFormatter(folderLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);

            var foldersCount = context.Folders.ToList().Count;
            var filesCount = context.Files.ToList().Count;
            Assert.AreEqual(2, foldersCount);
            Assert.AreEqual(1, filesCount);
            Assert.AreEqual(11, writer.Claims.Count);
        }




    }
}
