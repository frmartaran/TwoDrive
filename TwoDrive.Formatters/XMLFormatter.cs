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
using TwoDrive.Formatters.Extensions;
using System.Linq;

namespace TwoDrive.Formatters
{
    public class XMLFormatter : IFormatter<Folder>
    {
        public ILogic<Folder> LogicToSave { get; set; }
        public IFileLogic FileLogic { get; set; }
        public string FileExtension { get; set; }
        public Writer WriterFor { get; set; }

        public XMLFormatter(ILogic<Folder> logic, IFileLogic fileLogic)
        {
            LogicToSave = logic;
            FileLogic = fileLogic;
        }

        public void Import(string path)
        {
            var document = Load<XmlDocument>(path);
            var rootNode = document.DocumentElement;
            var root = CreateFolder(rootNode, "Root");
            LogicToSave.Create(root);
            WriterFor.AddRootClaims(root);
            var fileNodes = rootNode.GetElementsByTagName("File");
            foreach(XmlElement element in fileNodes)
            {
                ValidateDates(element, out DateTime creationDate, out DateTime dateModified);
                var nameAttribute = element.Attributes["name"];
                var name = nameAttribute.Value;
                var contentNode = element.GetElementsByTagName("Content");
                var file = new TxtFile
                {
                    CreationDate = creationDate,
                    DateModified = dateModified,
                    Name = name,
                    Content = contentNode.Item(0).InnerText,
                    Owner = WriterFor,
                    ParentFolder = root
                };
                FileLogic.Create(file);
                WriterFor.AddCreatorClaimsTo(file);
                
            }
            AddChildFolders(rootNode, root);

        }

        private void AddChildFolders(XmlElement parentNode, Folder parentFolder)
        {
            var childNodes = parentNode.GetElementsByTagName("Folder");
            if (childNodes.Count == 0)
                return;

            var folderChildren = childNodes.Cast<XmlElement>()
                .Where(e => e.ParentNode == parentNode)
                .ToList();

            foreach (XmlElement innerFolder in folderChildren)
            {
                var nameAttribute = innerFolder.Attributes["name"];
                if (nameAttribute == null)
                    throw new FormatterException("Each folder tag must have the name attribute");

                var name = nameAttribute.Value;
                var newFolder = CreateFolder(innerFolder, name);
                newFolder.ParentFolder = parentFolder;
                LogicToSave.Create(newFolder);
                WriterFor.AddCreatorClaimsTo(newFolder);

                AddChildFolders(innerFolder, newFolder);
            }
        }

        private Folder CreateFolder(XmlElement node, string name)
        {
            ValidateDates(node, out DateTime creationDate, out DateTime dateModified);
            var folder = new Folder
            {
                Owner = WriterFor,
                Name = name,
                CreationDate = creationDate,
                DateModified = dateModified,
                FolderChildren = new List<Element>()
            };
            return folder;
        }

        private static void ValidateDates(XmlElement node, out DateTime creationDate, out DateTime dateModified)
        {
            var creationDateNodes = node.GetElementsByTagName("CreationDate");
            var dateModifiedNodes = node.GetElementsByTagName("DateModified");
            node.ValidateDateNodes(creationDateNodes, dateModifiedNodes);

            var creationDateString = creationDateNodes.Item(0).InnerText;
            var dateModifiedString = dateModifiedNodes.Item(0).InnerText;

            var isCorrectCreationDate = DateTime.TryParse(creationDateString, out creationDate);
            var isCorrectDateModified = DateTime.TryParse(dateModifiedString, out dateModified);

            if (!isCorrectCreationDate)
                throw new FormatterException("Invalid date format. Please try: yyyy-mm-dd");

            if (!isCorrectDateModified)
                throw new FormatterException("Invalid date format. Please try: yyyy-mm-dd");
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
