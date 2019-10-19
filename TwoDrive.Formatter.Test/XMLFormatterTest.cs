using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Xml;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Formatter.Interface;
using TwoDrive.Formatters;
using TwoDrive.Formatters.Exceptions;

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

        [TestMethod]
        [ExpectedException(typeof(FormatterException))]
        public void FileNotFound()
        {
            var path = "path";
            var mockFolderLogic = new Mock<IFolderLogic>();
            var formatter = new XMLFormatter(mockFolderLogic.Object);
            var document = formatter.Load<XmlDocument>(path);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatterException))]
        public void WrongXMLFile()
        {
            string path = $@"{examplesRoot}\\Wrong File.xml";
            var mockFolderLogic = new Mock<IFolderLogic>();
            var formatter = new XMLFormatter(mockFolderLogic.Object);
            var document = formatter.Load<XmlDocument>(path);
        }
    }
}
