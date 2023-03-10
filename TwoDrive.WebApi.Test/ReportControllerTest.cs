using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Controllers;
using TwoDrive.WebApi.Models;

namespace TwoDrive.WebApi.Test
{
    [TestClass]
    public class ReportControllerTest
    {
        private Writer writer;
        private Writer secondWriter;

        [TestInitialize]
        public void SetUp()
        {
            writer = new Writer()
            {
                Id = 2,
                UserName = "Writer",
                Password = "132",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<Writer>()
            };

            secondWriter = new Writer()
            {
                Id = 3,
                UserName = "Second Writer",
                Password = "1325",
                Role = Role.Writer,
                Claims = new List<CustomClaim>(),
                Friends = new List<Writer>()
            };
        }
        [TestMethod]
        public void GetFileModificationReport()
        {
            
            var file = new TxtFile
            {
                Id = 1,
                Owner = writer
            };
            var secondFile = new TxtFile
            {
                Id = 2,
                Owner = secondWriter
            };
            var firstModification = new Modification
            {
                ElementModified = file,
                type = ModificationType.Changed
            };
            var secondModification = new Modification
            {
                ElementModified = file,
                type = ModificationType.Changed
            };
            var thirdModification = new Modification
            {
                ElementModified = secondFile,
                type = ModificationType.Changed
            };
            var allModifications = new List<Modification>
            {
                firstModification,
                secondModification,
                thirdModification
            };
            var groupedList = allModifications
                .GroupBy(g => g.ElementModified)
                .ToList();
            var mockFileLogic = new Mock<ILogic<File>>();
            var mockLogic = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAllFromDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(groupedList);
            var controller = new ReportController(mockLogic.Object, mockFileLogic.Object);
            var start = new DateTime(2019, 3, 23);
            var end = new DateTime(2019, 5, 10);
            var result = controller.GetFileModificationReport(start, end);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetFileModificationReportWrongRange()
        {
            var mockFileLogic = new Mock<ILogic<File>>();
            var mockLogic = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAllFromDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new LogicException(""));
            var controller = new ReportController(mockLogic.Object, mockFileLogic.Object);
            var start = new DateTime(2019, 3, 23);
            var end = new DateTime(2019, 5, 10);
            var result = controller.GetFileModificationReport(end, start);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void GetEmptyFileModificationReport()
        {
            var allModifications = new List<Modification>();
            var groupedList = allModifications
                .GroupBy(g => g.ElementModified)
                .ToList();
            var mockFileLogic = new Mock<ILogic<File>>();
            var mockLogic = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAllFromDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(groupedList);
            var controller = new ReportController(mockLogic.Object, mockFileLogic.Object);
            var start = new DateTime(2019, 3, 23);
            var end = new DateTime(2019, 5, 10);
            var result = controller.GetFileModificationReport(start, end);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetTopWriters()
        {
            var writer = new Writer
            {
                Id = 1,
                UserName = "Owner 1"
            };
            var file = new TxtFile
            {
                Name = "File One",
                Owner = writer
            };
            var fileTwo = new TxtFile
            {
                Name = "File Two",
                Owner = writer
            };
            var friend = new Writer
            {
                Id = 1,
                UserName = "Owner 2"
            };
            var fileThree = new TxtFile
            {
                Name = "File Three",
                Owner = friend
            };
            var files = new List<File>
            {
                file,
                fileThree,
                fileTwo
            };
            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAll())
                .Returns(files);
            var mockModificationLogic = new Mock<IModificationLogic>();
            var controller = new ReportController(mockModificationLogic.Object, mockLogic.Object);
            var result = controller.GetTopWriters();
            var okResult = result as OkObjectResult;
            var topWriters = okResult.Value as List<TopWriterModel>;

            var topWriter = topWriters.FirstOrDefault();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(writer.UserName, topWriter.Username);
            Assert.AreEqual(2, topWriter.FileCount);
        }

        [TestMethod]
        public void GetNoTopWriters()
        {
            var files = new List<File>();
            var mockLogic = new Mock<ILogic<File>>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAll())
                .Returns(files);
            var mockModificationLogic = new Mock<IModificationLogic>();
            var controller = new ReportController(mockModificationLogic.Object, mockLogic.Object);
            var result = controller.GetTopWriters();
            var okResult = result as OkObjectResult;

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(okResult.Value, "There are no top writers yet");
        }

        [TestMethod]
        public void GetFolderModificationReport()
        {
            var folder = new Folder
            {
                Id = 1,
                Owner = writer
            };
            var secondFolder = new Folder
            {
                Id = 2,
                Owner = secondWriter
            };
            var firstModification = new Modification
            {
                Date = new DateTime(2019, 4, 2),
                ElementModified = folder,
                type = ModificationType.Changed
            };
            var secondModification = new Modification
            {
                Date = new DateTime(2019, 4, 2),
                ElementModified = folder,
                type = ModificationType.Changed
            };
            var thirdModification = new Modification
            {
                Date = new DateTime(2019, 4, 2),
                ElementModified = secondFolder,
                type = ModificationType.Changed
            };
            var allModifications = new List<Modification>
            {
                firstModification,
                secondModification,
                thirdModification
            };
            var groupedList = allModifications
                .GroupBy(g => g.ElementModified)
                .ToList();
            var mockFileLogic = new Mock<ILogic<File>>();
            var mockLogic = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAllFromDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(groupedList);
            var controller = new ReportController(mockLogic.Object, mockFileLogic.Object);
            var start = new DateTime(2019, 3, 23);
            var end = new DateTime(2019, 5, 10);
            var result = controller.GetFolderModificationReport(start, end);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
