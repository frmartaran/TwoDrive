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
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.DataAccess;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Importer;
using TwoDrive.Importer.Parameters;
using TwoDrive.Importers;

namespace TwoDrive.BusinessLogic.Test.LogicTest
{
    [TestClass]
    public class ImporterLogicTest
    {
        private Writer writer;

        private const string examplesRootForJson = "..\\..\\..\\Json Tree Examples";
        private const string examplesRootForXML = "..\\..\\..\\Xml Tree Examples";
        private XMLParameters xmlParameters;


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
            xmlParameters = new XMLParameters
            {
                Path = ""
            };
            
        }

        [TestMethod]
        public void GetImporter()
        {
            var mockFolderLogic = new Mock<IFolderLogic>();
            var mockFileLogic = new Mock<IFileLogic>();
            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var mockModificationLogic = new Mock<IModificationLogic>().Object;
            var dependencies = new ImporterDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object, mockModificationLogic);
           
            var options = new ImportingOptions
            {
                ImporterName = "XML",
                Owner = writer,
                Parameters = xmlParameters
            };
            var importerLogic = new ImporterLogic(dependencies);
            importerLogic.Options = options;
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
            var dependencies = new ImporterDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object, mockModificationLogic);
            var options = new ImportingOptions
            {
                ImporterName = "JSON",
                Owner = writer,
                Parameters = xmlParameters
            };
            var importerLogic = new ImporterLogic(dependencies);
            importerLogic.Options = options;

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

            var dependencies = new ImporterDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object, mockModificationLogic);
            var options = new ImportingOptions
            {
                ImporterName = "txt",
                Owner = writer,
                Parameters = xmlParameters
            };
            var importerLogic = new ImporterLogic(dependencies);
            importerLogic.Options = options;

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

            var dependencies = new ImporterDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object, mockModificationLogic.Object);
            var parameters = new XMLParameters
            {
                Path = $"{examplesRootForXML}\\One Folder.xml"
            };
            var options = new ImportingOptions
            {
                ImporterName = "XML",
                Owner = writer,
                Parameters = parameters
            };
            var importerLogic = new ImporterLogic(dependencies);
            importerLogic.Options = options;
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
            var importerDependecies = new ImporterDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var parameters = new XMLParameters
            {
                Path = $"{examplesRootForXML}\\One Folder.xml"
            };
            var options = new ImportingOptions
            {
                ImporterName = "XML",
                Owner = writer,
                Parameters = parameters
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(importerDependecies);
            importerLogic.Options = options;
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
            var importerDependecies = new ImporterDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var parameters = new XMLParameters
            {
                Path = $"{examplesRootForXML}\\Simple Tree With File.xml"
            };
            var options = new ImportingOptions
            {
                ImporterName = "XML",
                Owner = writer,
                Parameters = parameters
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(importerDependecies);
            importerLogic.Options = options;

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
            var importerDependecies = new ImporterDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var parameters = new XMLParameters
            {
                Path = $"{examplesRootForXML}\\Two Types Of Files.xml"
            };
            var options = new ImportingOptions
            {
                ImporterName = "XML",
                Owner = writer,
                Parameters = parameters
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(importerDependecies);
            importerLogic.Options = options;

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
            var importerDependecies = new ImporterDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var parameters = new XMLParameters
            {
                Path = $"{examplesRootForXML}\\Unsupported File Type.xml"
            };
            var options = new ImportingOptions
            {
                ImporterName = "XML",
                Owner = writer,
                Parameters = parameters
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(importerDependecies);
            importerLogic.Options = options;

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
            var importerDependecies = new ImporterDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var parameters = new XMLParameters
            {
                Path = $"{examplesRootForXML}\\Two Level Tree With File.xml"
            };
            var options = new ImportingOptions
            {
                ImporterName = "XML",
                Owner = writer,
                Parameters = parameters
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(importerDependecies);
            importerLogic.Options = options;

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

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ImportMissingWriter()
        {
            var context = ContextFactory.GetMemoryContext("Import Missing Writer");
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
            var importerDependecies = new ImporterDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var parameters = new XMLParameters
            {
                Path = $"{examplesRootForXML}\\Two Level Tree With File.xml"
            };
            var options = new ImportingOptions
            {
                ImporterName = "XML",
                Parameters = parameters
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(importerDependecies);
            importerLogic.Options = options;

            importerLogic.Import();

        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ImportWhenRootAlreadyExists()
        {
            var context = ContextFactory.GetMemoryContext("Import when root already exists");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var fileValidator = new Mock<IValidator<Element>>().Object;
            var validator = new FolderValidator(folderRepository);
            var modificationRepository = new ModificationRepository(context);
            var modificationsLogic = new ModificationLogic(modificationRepository);
            var folderDependecies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var writerRepository = new WriterRepository(context);
            var writerValidator = new Mock<IValidator<Writer>>().Object;

            var folderLogic = new FolderLogic(folderDependecies);
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            var writerLogic = new WriterLogic(writerRepository, writerValidator);
            var importerDependecies = new ImporterDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var parameters = new XMLParameters
            {
                Path = $"{examplesRootForXML}\\Two Level Tree With File.xml"
            };
            var options = new ImportingOptions
            {
                ImporterName = "XML",
                Owner = writer,
                Parameters = parameters
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var root = new Folder
            {
                CreationDate = new DateTime(2019, 3, 15),
                DateModified = new DateTime(2019, 3, 15),
                FolderChildren = new List<Element>(),
                Owner = writer,
                Name = "Root",
                ParentFolder = null,
            };

            folderLogic.Create(root);

            var importerLogic = new ImporterLogic(importerDependecies);
            importerLogic.Options = options;

            importerLogic.Import();
        }

        [TestMethod]
        [ExpectedException(typeof(LogicException))]
        public void ImportedChildFailsValidation()
        {
            var context = ContextFactory.GetMemoryContext("Import child fails validation");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var fileValidator = new Mock<IValidator<Element>>().Object;
            var validator = new FolderValidator(folderRepository);
            var modificationRepository = new ModificationRepository(context);
            var modificationsLogic = new ModificationLogic(modificationRepository);
            var folderDependecies = new ElementLogicDependencies(folderRepository, fileRepository,
                validator, modificationRepository);
            var writerRepository = new WriterRepository(context);
            var writerValidator = new Mock<IValidator<Writer>>().Object;

            var folderLogic = new FolderLogic(folderDependecies);
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            var writerLogic = new WriterLogic(writerRepository, writerValidator);
            var importerDependecies = new ImporterDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var parameters = new XMLParameters
            {
                Path = $"{examplesRootForXML}\\Validation Error.xml"
            };
            var options = new ImportingOptions
            {
                ImporterName = "XML",
                Owner = writer,
                Parameters = parameters
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(importerDependecies);
            importerLogic.Options = options;

            importerLogic.Import();
        }

        [TestMethod]
        public void ImportAFolderWithTwoTypesOfFilesJSON()
        {
            var context = ContextFactory.GetMemoryContext("Import A Folder with two types of files JSON");
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
            var importerDependecies = new ImporterDependencies(folderLogic, fileLogic, writerLogic,
                modificationsLogic);
            var parameters = new JsonParameter
            {
                Path = $"{examplesRootForJson}\\baseCase.json"
            };
            var options = new ImportingOptions
            {
                ImporterName = "JSON",
                Owner = writer,
                Parameters = parameters
            };
            writerRepository.Insert(writer);
            writerRepository.Save();

            var importerLogic = new ImporterLogic(importerDependecies);
            importerLogic.Options = options;

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
        public void GetAllImporters()
        {
            var mockFolderLogic = new Mock<IFolderLogic>().Object;
            var mockFileLogic = new Mock<IFileLogic>().Object;
            var mockWriterLogic = new Mock<ILogic<Writer>>().Object;
            var mockModificationLogic = new Mock<IModificationLogic>().Object;

            var dependencies = new ImporterDependencies(mockFolderLogic, mockFileLogic, 
                mockWriterLogic, mockModificationLogic);

            var importerLogic = new ImporterLogic(dependencies);
            var importersInfo = importerLogic.GetAllImporters();
            var names = importersInfo.Select(ii => ii.Name).ToList();
            var jsonParam = importersInfo.Select(ii => ii.Parameters)
                .OfType<JsonParameter>()
                .FirstOrDefault();
            var xmlParam = importersInfo.Select(ii => ii.Parameters)
                .OfType<XMLParameters>()
                .FirstOrDefault();
            Assert.IsNotNull(importersInfo);
            Assert.IsTrue(names.Contains("XML"));
            Assert.IsTrue(names.Contains("JSON"));
            Assert.IsNotNull(jsonParam);
            Assert.IsNotNull(xmlParam);


        }
    }
}
