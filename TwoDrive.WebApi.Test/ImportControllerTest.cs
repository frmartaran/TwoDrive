using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.WebApi.Controllers;

namespace TwoDrive.WebApi.Test
{
    [TestClass]
    public class ImportControllerTest
    {
        Writer writer;
        [TestInitialize]
        public void Setup()
        {
            writer = new Writer
            {
                Id = 1
            };
        }

        [TestMethod]
        public void SuccessfullImport()
        {
            var mockImportLogic = new Mock<IImporterLogic>(MockBehavior.Strict);
            mockImportLogic.Setup(m => m.Import());

            var folderLogic = new Mock<IFolderLogic>().Object;
            var fileLogic = new Mock<IFileLogic>().Object;
            var modificationLogic = new Mock<IModificationLogic>().Object;
            var mockWriterLogic = new Mock<ILogic<Writer>>();
            mockWriterLogic.Setup(m => m.Get(It.IsAny<int>()));
            var dependencies = new ImporterDependencies(folderLogic, fileLogic,
                mockWriterLogic.Object, modificationLogic);

            var controller = new ImportController(dependencies);
            var result = controller.Import("", "XML", writer.Id);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            mockImportLogic.VerifyAll();
        }
    }
}
