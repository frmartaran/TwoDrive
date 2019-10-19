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
        private const string examplesRoot = "..\\..\\..\\Xml Tree Examples";


        [TestMethod]
        public void SuccessfullyLoadFile()
        {
            string path = $@"{examplesRoot}\\Single Folder.xml";
            var mockFolderLogic = new Mock<IFolderLogic>();
            var formatter = new XMLFormatter(mockFolderLogic.Object);
            var document = formatter.Load<XmlDocument>(path);
            Assert.IsNotNull(document);
        }
    }
}
