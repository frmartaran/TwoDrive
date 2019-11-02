using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Importer;
using TwoDrive.Importers;

namespace TwoDrive.BusinessLogic.Test.LogicTest
{
    [TestClass]
    public class ImporterLogicTest
    {
        private Writer writer;

        private const string examplesRootForJson = "..\\..\\..\\Json Tree Examples";
        private const string examplesRootForXML = "..\\..\\..\\Xml Tree Examples";


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
            var mockModificationLogic = new Mock<IModificationLogic>().Object;
            var dependencies = new ImporterLogicDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object, mockModificationLogic);
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
            var mockModificationLogic = new Mock<IModificationLogic>().Object;
            var dependencies = new ImporterLogicDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object, mockModificationLogic);
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

        [TestMethod]
        [ExpectedException(typeof(ImporterNotFoundException))]
        public void WrongImporterType()
        {
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockFileLogic = new Mock<IFileLogic>();
            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModificationLogic = new Mock<IModificationLogic>().Object;

            var dependencies = new ImporterLogicDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object, mockModificationLogic);
            var options = new ImportingOptions
            {
                FilePath = "",
                FileType = "txt",
                Owner = writer
            };
            var importerLogic = new ImporterLogic(options, dependencies);
            var importer = importerLogic.GetImporter();

        }

        [TestMethod]
        public void ImportAFolderMock()
        {
            var mockFolderLogic = new Mock<IFolderLogic>(MockBehavior.Strict);
            mockFolderLogic.Setup(m => m.Create(It.IsAny<Folder>()));
            var mockFileLogic = new Mock<IFileLogic>();
            var mockWriterLogic = new Mock<ILogic<Writer>>(MockBehavior.Strict);
            mockWriterLogic.Setup(m => m.Update(It.IsAny<Writer>()));
            var mockModificationLogic = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockModificationLogic.Setup(m => m.Create(It.IsAny<Modification>()));

            var dependencies = new ImporterLogicDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object, mockModificationLogic.Object);
            var options = new ImportingOptions
            {
                FilePath = $"{examplesRootForXML}\\One Folder.xml",
                FileType = "XML",
                Owner = writer
            };
            var importerLogic = new ImporterLogic(options, dependencies);
            importerLogic.Import();

            mockFolderLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
            mockModificationLogic.VerifyAll();
        }

        [TestMethod]
        public void ImportAFolder()
        {
            var context = ContextFactory.GetMemoryContext("Import A Folder");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var fileValidator = new Mock<IValidator<Element>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var modificationRepository = new ModificationRepository(context);
            var modificationsLogic = new ModificationLogic(modificationRepository);
            var folderDependecies = new ElementLogicDependencies(folderRepository, fileRepository, 
                validator, modificationRepository);
            var writerRepository = new WriterRepository(context);
            var writerValidator = new Mock<IValidator<Writer>>().Object;

            var folderLogic = new FolderLogic(folderDependecies);
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            var writerLogic = new WriterLogic(writerRepository, writerValidator);
            var importerDependecies = new ImporterLogicDependencies(folderLogic, fileLogic, writerLogic, 
                modificationsLogic);
            var options = new ImportingOptions
            {
                FilePath = $"{examplesRootForXML}\\One Folder.xml",
                FileType = "XML",
                Owner = writer
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(options, importerDependecies);
            importerLogic.Import();

            var foldersInDb = folderLogic.GetAll();
            var claims = writer.Claims.Count;
            var modificationsCount = context.Modifications.ToList().Count;
            Assert.AreEqual(1, foldersInDb.Count);
            Assert.AreEqual(3, claims);
            Assert.AreEqual(1, modificationsCount);
            
        }

        [TestMethod]
        public void ImportAFolderWithAFile()
        {
            var context = ContextFactory.GetMemoryContext("Import A Folder with A File");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var fileValidator = new Mock<IValidator<Element>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var modificationRepository = new ModificationRepository(context);
            var modificationsLogic = new ModificationLogic(modificationRepository);
            var folderDependecies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var writerRepository = new WriterRepository(context);
            var writerValidator = new Mock<IValidator<Writer>>().Object;

            var folderLogic = new FolderLogic(folderDependecies);
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            var writerLogic = new WriterLogic(writerRepository, writerValidator);
            var importerDependecies = new ImporterLogicDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var options = new ImportingOptions
            {
                FilePath = $"{examplesRootForXML}\\Simple Tree With File.xml",
                FileType = "XML",
                Owner = writer
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(options, importerDependecies);
            importerLogic.Import();

            var foldersInDb = folderLogic.GetAll();
            var claims = writer.Claims.Count;
            var modificationsCount = context.Modifications.Count();
            var filesCount = context.Files.Count();
            Assert.AreEqual(2, foldersInDb.Count);
            Assert.AreEqual(1, filesCount);
            Assert.AreEqual(11, claims);
            Assert.AreEqual(5, modificationsCount);

        }

        [TestMethod]
        public void ImportAFolderWithTwoTypesOfFiles()
        {
            var context = ContextFactory.GetMemoryContext("Import A Folder with two types of files");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var fileValidator = new Mock<IValidator<Element>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var modificationRepository = new ModificationRepository(context);
            var modificationsLogic = new ModificationLogic(modificationRepository);
            var folderDependecies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var writerRepository = new WriterRepository(context);
            var writerValidator = new Mock<IValidator<Writer>>().Object;

            var folderLogic = new FolderLogic(folderDependecies);
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            var writerLogic = new WriterLogic(writerRepository, writerValidator);
            var importerDependecies = new ImporterLogicDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var options = new ImportingOptions
            {
                FilePath = $"{examplesRootForXML}\\Two Types Of Files.xml",
                FileType = "XML",
                Owner = writer
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(options, importerDependecies);
            importerLogic.Import();

            var foldersInDb = folderLogic.GetAll();
            var claims = writer.Claims.Count;
            var modificationsCount = context.Modifications.Count();
            var txtFileCount = context.Txts.Count();
            var htmlFileCount = context.Htmls.Count();
            var filesCount = context.Files.Count();
            Assert.AreEqual(2, foldersInDb.Count);
            Assert.AreEqual(2, filesCount);
            Assert.AreEqual(1, txtFileCount);
            Assert.AreEqual(1, htmlFileCount);
            Assert.AreEqual(15, claims);
            Assert.AreEqual(7, modificationsCount);

        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ImportAFolderUnsupportedTypeOfFile()
        {
            var context = ContextFactory.GetMemoryContext("Import A Folder with Unsupported type of file");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var fileValidator = new Mock<IValidator<Element>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var modificationRepository = new ModificationRepository(context);
            var modificationsLogic = new ModificationLogic(modificationRepository);
            var folderDependecies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var writerRepository = new WriterRepository(context);
            var writerValidator = new Mock<IValidator<Writer>>().Object;

            var folderLogic = new FolderLogic(folderDependecies);
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            var writerLogic = new WriterLogic(writerRepository, writerValidator);
            var importerDependecies = new ImporterLogicDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var options = new ImportingOptions
            {
                FilePath = $"{examplesRootForXML}\\Unsupported File Type.xml",
                FileType = "XML",
                Owner = writer
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(options, importerDependecies);
            importerLogic.Import();
        }

        [TestMethod]
        public void ImportTwoLevelTree()
        {
            var context = ContextFactory.GetMemoryContext("Import A Two Level Tree");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var fileValidator = new Mock<IValidator<Element>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var modificationRepository = new ModificationRepository(context);
            var modificationsLogic = new ModificationLogic(modificationRepository);
            var folderDependecies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var writerRepository = new WriterRepository(context);
            var writerValidator = new Mock<IValidator<Writer>>().Object;

            var folderLogic = new FolderLogic(folderDependecies);
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            var writerLogic = new WriterLogic(writerRepository, writerValidator);
            var importerDependecies = new ImporterLogicDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var options = new ImportingOptions
            {
                FilePath = $"{examplesRootForXML}\\Two Level Tree With File.xml",
                FileType = "XML",
                Owner = writer
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(options, importerDependecies);
            importerLogic.Import();

            var foldersInDb = folderLogic.GetAll();
            var claims = writer.Claims.Count;
            var modificationsCount = context.Modifications.Count();
            var txtFileCount = context.Txts.Count();
            var htmlFileCount = context.Htmls.Count();
            var filesCount = context.Files.Count();
            Assert.AreEqual(3, foldersInDb.Count);
            Assert.AreEqual(1, filesCount);
            Assert.AreEqual(1, htmlFileCount);
            Assert.AreEqual(15, claims);
            Assert.AreEqual(10, modificationsCount);

        }
    }
}
