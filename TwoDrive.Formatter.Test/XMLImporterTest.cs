using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TwoDrive.BusinessLogic;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Importers;
using TwoDrive.Importers.Exceptions;

namespace TwoDrive.Importer.Test
{
    [TestClass]
    public class XMLImporterTest
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
            var mockFolderLogic = new Mock<IFolderLogic>().Object;
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var formatter = new XMLImporter(mockFolderLogic, mockFileLogic);
            var document = formatter.Load<XmlDocument>(path);
            Assert.IsNotNull(document);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void FileNotFound()
        {
            var path = "path";
            var mockFolderLogic = new Mock<IFolderLogic>().Object;
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var formatter = new XMLImporter(mockFolderLogic, mockFileLogic);
            var document = formatter.Load<XmlDocument>(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WrongXMLFile()
        {
            var path = $@"{examplesRoot}\\Wrong File.xml";
            var mockFolderLogic = new Mock<IFolderLogic>().Object;
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var formatter = new XMLImporter(mockFolderLogic, mockFileLogic);
            var document = formatter.Load<XmlDocument>(path);
        }

        [TestMethod]
        public void SaveRootMock()
        {
            var path = $@"{examplesRoot}\\Single Folder.xml";
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            var mockFileLogic = new Mock<IFileLogic>().Object;
            mockFolderLogic.Setup(m => m.Create(It.IsAny<Folder>()));
            var formatter = new XMLImporter(mockFolderLogic.Object, mockFileLogic)
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
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository, 
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLImporter(folderLogic, mockFileLogic)
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
        [ExpectedException(typeof(ImporterException))]
        public void WithoutCreationDate()
        {
            var path = $@"{examplesRoot}\\WithoutCreationDate.xml";
            var context = ContextFactory.GetMemoryContext("Without Creation Date");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLImporter(folderLogic, mockFileLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void WithoutDateModified()
        {
            var path = $@"{examplesRoot}\\WithoutDateModified.xml";
            var context = ContextFactory.GetMemoryContext("Without Creation Date");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLImporter(folderLogic, mockFileLogic)
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
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLImporter(folderLogic, mockFileLogic)
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
        [ExpectedException(typeof(ImporterException))]
        public void InvalidCreationDate()
        {
            var path = $@"{examplesRoot}\\InvalidCreationDate.xml";
            var context = ContextFactory.GetMemoryContext("Without Creation Date");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLImporter(folderLogic, mockFileLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);
        }

        [TestMethod]
        [ExpectedException(typeof(ImporterException))]
        public void InvalidDateModified()
        {
            var path = $@"{examplesRoot}\\InvalidDateModified.xml";
            var context = ContextFactory.GetMemoryContext("Without Creation Date");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLImporter(folderLogic, mockFileLogic)
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
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLImporter(folderLogic, mockFileLogic)
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
        [ExpectedException(typeof(ImporterException))]
        public void FolderWithNoName()
        {
            var path = $@"{examplesRoot}\\NoName.xml";
            var context = ContextFactory.GetMemoryContext("No Name");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new Mock<IFileRepository>().Object;
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var formatter = new XMLImporter(folderLogic, mockFileLogic)
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
            var fileRepository = new FileRepository(context);
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var fileValidator = new Mock<IValidator<Element>>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            var formatter = new XMLImporter(folderLogic, fileLogic)
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

        [TestMethod]
        public void SaveSimpleTreeWithTwoTypeOfFile()
        {
            var path = $@"{examplesRoot}\\Two Types Of Files.xml";
            var context = ContextFactory.GetMemoryContext("Two Types Of File");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var modificationRepository = new Mock<IRepository<Modification>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var fileValidator = new Mock<IValidator<Element>>().Object;
            var dependencies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var folderLogic = new FolderLogic(dependencies);
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            var formatter = new XMLImporter(folderLogic, fileLogic)
            {
                WriterFor = writer
            };
            context.Writers.Add(writer);
            context.SaveChanges();
            formatter.Import(path);

            var foldersCount = context.Folders.ToList().Count;
            var filesCount = context.Files.ToList().Count;
            var txtfile = context.Files.FirstOrDefault();
            var htmlfile = context.Files.LastOrDefault();
            Assert.AreEqual(2, foldersCount);
            Assert.AreEqual(2, filesCount);
            Assert.IsInstanceOfType(txtfile, typeof(TxtFile));
            Assert.IsInstanceOfType(htmlfile, typeof(HTMLFile));
            Assert.AreEqual(11, writer.Claims.Count);
        }

    }
}
