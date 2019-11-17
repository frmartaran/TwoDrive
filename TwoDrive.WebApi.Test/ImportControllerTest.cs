using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.Domain;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Parameters;
using TwoDrive.WebApi.Controllers;

namespace TwoDrive.WebApi.Test
{
    [TestClass]
    public class ImportControllerTest
    {
        Writer writer;

        ImportingParameters parameters;

        [TestInitialize]
        public void Setup()
        {
            writer = new Writer
            {
                Id = 1
            };
            parameters = new XMLParameters
            {
                Path = ""
            };

        }

        [TestMethod]
        public void SuccessfullImport()
        {
            var mockImportLogic = new Mock<IImporterLogic>(MockBehavior.Strict);
            mockImportLogic.Setup(m => m.Import(""));
            mockImportLogic.SetupSet(m => m.Options = It.IsAny<ImportingOptions>());

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(writer);

            var controller = new ImportController(mockImportLogic.Object, mockWriterLogic.Object);
            var result = controller.Import(parameters, "XML", writer.Id);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockImportLogic.VerifyAll();
            mockWriterLogic.VerifyAll();

        }

        [TestMethod]
        public void OwnerNotFound()
        {
            var mockImportLogic = new Mock<IImporterLogic>();

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns<Writer>(null);

            var controller = new ImportController(mockImportLogic.Object, mockWriterLogic.Object);
            var result = controller.Import(parameters, "XML", writer.Id);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            mockImportLogic.VerifyAll();
            mockWriterLogic.VerifyAll();
        }

        [TestMethod]
        public void ImporterNotFound()
        {
            var mockImportLogic = new Mock<IImporterLogic>(MockBehavior.Strict);
            mockImportLogic.Setup(m => m.Import(""))
                .Throws(new ImporterNotFoundException(""));

            mockImportLogic.SetupSet(m => m.Options = It.IsAny<ImportingOptions>());

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(writer);

            var controller = new ImportController(mockImportLogic.Object, mockWriterLogic.Object);
            var result = controller.Import(parameters, "txt", writer.Id);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            mockImportLogic.VerifyAll();
            mockWriterLogic.VerifyAll();

        }

        [TestMethod]
        public void ImportingError()
        {
            var mockImportLogic = new Mock<IImporterLogic>(MockBehavior.Strict);
            mockImportLogic.Setup(m => m.Import(""))
                .Throws(new LogicException(""));

            mockImportLogic.SetupSet(m => m.Options = It.IsAny<ImportingOptions>());

            var mockWriterLogic = new Mock<ILogic<Writer>>();
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()))
                .Returns(writer);

            var controller = new ImportController(mockImportLogic.Object, mockWriterLogic.Object);
            var result = controller.Import(parameters, "txt", writer.Id);

            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            mockImportLogic.VerifyAll();
            mockWriterLogic.VerifyAll();

        }

        [TestMethod]
        public void GetAllImporters()
        {
            var infoXml = new ImporterInfo
            {
                Name = "XML",
                Parameters = new XMLParameters()
            };
            var infoJSON = new ImporterInfo
            {
                Name = "Json",
                Parameters = new JsonParameter()
            };

            var allImporters = new List<ImporterInfo>
            {
                infoXml,
                infoJSON
            };

            var mockImportLogic = new Mock<IImporterLogic>(MockBehavior.Strict);
            mockImportLogic.Setup(m => m.GetAllImporters(""))
                .Returns(allImporters);
            var mockWriterLogic = new Mock<ILogic<Writer>>().Object;

            var controller = new ImportController(mockImportLogic.Object, mockWriterLogic);
            var result = controller.Get();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockImportLogic.VerifyAll();

        }
    }
}
