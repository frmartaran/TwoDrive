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
        [TestMethod]
        public void GetModificationReport()
        {
            var file = new TxtFile
            {
                Id = 1
            };
            var secondFile = new TxtFile
            {
                Id = 2
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
            var result = controller.GetModificationReport(start, end);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetModificationReportWrongRange()
        {
            var mockFileLogic = new Mock<ILogic<File>>();
            var mockLogic = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAllFromDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Throws(new LogicException(""));
            var controller = new ReportController(mockLogic.Object, mockFileLogic.Object);
            var start = new DateTime(2019, 3, 23);
            var end = new DateTime(2019, 5, 10);
            var result = controller.GetModificationReport(end, start);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void GetEmptyModificationReport()
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
            var result = controller.GetModificationReport(start, end);

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
    }
}
