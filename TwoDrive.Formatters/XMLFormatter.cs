using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Formatter.Interface;
using TwoDrive.Formatters.Exceptions;

namespace TwoDrive.Formatters
{
    public class XMLFormatter : IFormatter<Folder>
    {
        public ILogic<Folder> LogicToSave { get; set; }
        public string FileExtension { get; set; }
        public Writer WriterFor { get; set; }

        public XMLFormatter(ILogic<Folder> logic)
        {
            LogicToSave = logic;
        }

        public void Import(string path)
        {
            var document = Load<XmlDocument>(path);
            var rootNode = document.DocumentElement;
            var root = CreateFolder(rootNode, "Root");
            WriterFor.AddRootClaims(root);

            var folderChildren = rootNode.GetElementsByTagName("Folder");
            foreach (XmlElement innerFolder in folderChildren)
            {
                var name = innerFolder.Attributes["name"].Value;
                var newFolder = CreateFolder(innerFolder, name);
                WriterFor.AddCreatorClaimsTo(newFolder);
            }

        }

        private Folder CreateFolder(XmlElement node, string name)
        {
            var creationDateNodes = node.GetElementsByTagName("CreationDate");
            if (!NodeExists(node, creationDateNodes))
                throw new FormatterException("Missing Creation Date Tag");

            var creationDateString = creationDateNodes.Item(0).InnerText;
            DateTime.TryParse(creationDateString, out DateTime creationDate);

            var dateModifiedNodes = node.GetElementsByTagName("DateModified");
            if (!NodeExists(node, dateModifiedNodes))
                throw new FormatterException("Missing Date Modified Tag");

            var dateModifiedString = dateModifiedNodes.Item(0).InnerText;
            DateTime.TryParse(dateModifiedString, out DateTime dateModified);

            var folder = new Folder
            {
                Owner = WriterFor,
                Name = name,
                CreationDate = creationDate,
                DateModified = dateModified,
                FolderChildren = new List<Element>()
            };

            LogicToSave.Create(folder);
            return folder;
        }

        private static bool NodeExists(XmlElement rootNode, XmlNodeList nodeList)
        {
            return nodeList.Count != 0 && nodeList.Item(0).ParentNode == rootNode;
        }

        public T Load<T>(string path) where T : class
        {
            try
            {
                var document = new XmlDocument();
                document.Load(path);
                return document as T;
            }
            catch (FileNotFoundException exception)
            {
                throw new FormatterException(exception.Message, exception);
            }
            catch (XmlException exception)
            {
                throw new FormatterException(exception.Message, exception);
            }
        }
    }
}
