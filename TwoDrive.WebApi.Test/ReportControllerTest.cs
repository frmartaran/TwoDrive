using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Logic;
using TwoDrive.Domain.FileManagement;
using TwoDrive.WebApi.Controllers;

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
            var mockLogic = new Mock<IModificationLogic>(MockBehavior.Strict);
            mockLogic.Setup(m => m.GetAllFromDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(groupedList);
            var controller = new ReportController(mockLogic.Object);
            var start = new DateTime(2019, 3, 23);
            var end = new DateTime(2019, 5, 10);
            var result = controller.GetModificationReport(start, end);

            mockLogic.VerifyAll();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }
    }
}
