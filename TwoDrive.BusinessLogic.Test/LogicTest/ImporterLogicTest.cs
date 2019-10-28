using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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
            var dependencies = new ImporterLogicDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object);
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
            var dependencies = new ImporterLogicDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object);
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
            var dependencies = new ImporterLogicDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object);
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
            var mockWriterLogic = new Mock<ILogic<Writer>>();
            var dependencies = new ImporterLogicDependencies(mockFolderLogic.Object,
                mockFileLogic.Object, mockWriterLogic.Object);
            var options = new ImportingOptions
            {
                FilePath = $"{examplesRootForXML}\\One Folder.xml",
                FileType = "XML",
                Owner = writer
            };
            var importerLogic = new ImporterLogic(options, dependencies);
            importerLogic.Import();

            mockFolderLogic.VerifyAll();
        }

        [TestMethod]
        public void ImportAFolder()
        {
            var context = ContextFactory.GetMemoryContext("Import A Folder");
            var folderRepository = new FolderRepository(context);
            var fileRepository = new FileRepository(context);
            var fileValidator = new Mock<IValidator<Element>>().Object;
            var validator = new Mock<IFolderValidator>().Object;
            var modifications = new Mock<IRepository<Modification>>().Object;
            var folderDependecies = new ElementLogicDependencies(folderRepository, fileRepository, 
                validator, modifications);
            var writerRepository = new WriterRepository(context);
            var writerValidator = new Mock<IValidator<Writer>>().Object;

            var folderLogic = new FolderLogic(folderDependecies);
            var fileLogic = new FileLogic(fileRepository, fileValidator);
            var writerLogic = new WriterLogic(writerRepository, writerValidator);
            var importerDependecies = new ImporterLogicDependencies(folderLogic, fileLogic, writerLogic);
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
            Assert.AreEqual(1, foldersInDb.Count);
            
        }


    }
}
