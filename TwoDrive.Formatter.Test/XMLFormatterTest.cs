using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Xml;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Formatter.Interface;
using TwoDrive.Formatters;

namespace TwoDrive.Formatter.Test
{
    [TestClass]
    public class XMLFormatterTest
    {
        [TestMethod]
        public void SuccessfullyLoadFileMock()
        {
            string path = "Some path";
            var mockFormatter = new Mock<IFormatter<Folder>>(MockBehavior.Strict);
            mockFormatter.Setup(m => m.Load<XmlDocument>(It.IsAny<string>()))
                .Returns(new XmlDocument());
            mockFormatter.Object.Import(path);
            mockFormatter.VerifyAll();
        }

        [TestMethod]
        public void SuccessfullyLoadFile()
        {
            string path = "~/Xml Tree Examples/Single Folder.xml";
            var mockFolderLogic = new Mock<IFolderLogic>();
            var formatter = new XMLFormatter(mockFolderLogic.Object);
            var document = formatter.Load<XmlDocument>(path);
        }
    }
}
