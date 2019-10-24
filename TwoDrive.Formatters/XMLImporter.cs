using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TwoDrive.Importer.Interface;
using TwoDrive.Importers.Exceptions;
using TwoDrive.Importers.Extensions;
using System.Linq;
using TwoDrive.Importer;
using TwoDrive.Importer.Interface.IFileManagement;
using TwoDrive.Importer.MockDomain;

namespace TwoDrive.Importers
{
    public class XMLImporter : IImporter<IFolder>
    {
        public string FileExtension
        {
            get
            {
                return "XML";
            }
        }


        public IFolder Import(string path)
        {
            var document = Load<XmlDocument>(path);
            var rootNode = document.DocumentElement;
            ValidateRootTag(rootNode);
            var root = CreateFolder(rootNode, ImporterConstants.Root);
            var fileNodes = rootNode.GetElementsByTagName(ImporterConstants.File);
            AddFiles(root, fileNodes);
            AddChildFolders(rootNode, root);
            return root;

        }

        private static void ValidateRootTag(XmlElement rootNode)
        {
            if (rootNode.Name != ImporterConstants.Root)
                throw new ImporterException(ImporterResource.NoRoot_Exception);
        }

        private void AddFiles(MockFolder parentFolder, XmlNodeList fileNodes)
        {
            var allFiles = new List<MockFile>();
            foreach (XmlElement element in fileNodes)
            {
                ValidateDates(element, out DateTime creationDate, out DateTime dateModified);
                var nameAttribute = element.Attributes[ImporterConstants.Name];
                ValidateNameAttribute(nameAttribute);
                var name = nameAttribute.Value;

                var typeAttribute = element.Attributes[ImporterConstants.Type];
                ValidateTypeAttribute(typeAttribute);
                var type = typeAttribute.Value;

                var contentNode = element.GetElementsByTagName(ImporterConstants.Content);
                ValidateContentTag(contentNode);

                var renderAttribute = element.Attributes["render"];
                var shouldRender = renderAttribute == null ? false : Convert.ToBoolean(renderAttribute.Value);

                var file = new MockFile
                {
                    Name = name,
                    CreationDate = creationDate,
                    DateModified = dateModified,
                    Content = contentNode.Item(0).InnerText,
                    Extension = type,
                    ParentFolder = parentFolder,
                    ShouldRender = shouldRender
                };
                allFiles.Add(file);
            }
            parentFolder.FolderChildren.AddRange(allFiles);
        }

        private static void ValidateContentTag(XmlNodeList contentNode)
        {
            if (contentNode.Count == 0)
                throw new ImporterException(ImporterResource.NoContent_Exception);
        }

        private static void ValidateTypeAttribute(XmlAttribute typeAttribute)
        {
            if (typeAttribute == null)
                throw new ImporterException(ImporterResource.NoType_Exception);
        }

        private static void ValidateNameAttribute(XmlAttribute nameAttribute)
        {
            if (nameAttribute == null)
                throw new ImporterException(ImporterResource.NoName_Exception);
        }

        private void AddChildFolders(XmlElement parentNode, MockFolder parentFolder)
        {
            var childNodes = parentNode.GetElementsByTagName(ImporterConstants.Folder);
            if (childNodes.Count == 0)
                return;

            var folderChildren = childNodes.Cast<XmlElement>()
                .Where(e => e.ParentNode == parentNode)
                .ToList();

            foreach (XmlElement innerFolder in folderChildren)
            {
                var nameAttribute = innerFolder.Attributes[ImporterConstants.Name];
                ValidateNameAttribute(nameAttribute);

                var name = nameAttribute.Value;
                var newFolder = CreateFolder(innerFolder, name);
                newFolder.ParentFolder = parentFolder;
                parentFolder.FolderChildren.Add(newFolder);
                var fileNodes = innerFolder.GetElementsByTagName(ImporterConstants.File);
                AddFiles(newFolder, fileNodes);
                AddChildFolders(innerFolder, newFolder);
            }
        }

        private MockFolder CreateFolder(XmlElement node, string name)
        {
            ValidateDates(node, out DateTime creationDate, out DateTime dateModified);
            var folder = new MockFolder
            {
                Name = name,
                CreationDate = creationDate,
                DateModified = dateModified,
                FolderChildren = new List<IElement>()
            };
            return folder;
        }

        private static void ValidateDates(XmlElement node, out DateTime creationDate, out DateTime dateModified)
        {
            var creationDateNodes = node.GetElementsByTagName(ImporterConstants.CreationDate);
            var dateModifiedNodes = node.GetElementsByTagName(ImporterConstants.DateModified);
            node.ValidateDateNodes(creationDateNodes, dateModifiedNodes);

            var creationDateString = creationDateNodes.Item(0).InnerText;
            var dateModifiedString = dateModifiedNodes.Item(0).InnerText;

            var isCorrectCreationDate = DateTime.TryParse(creationDateString, out creationDate);
            var isCorrectDateModified = DateTime.TryParse(dateModifiedString, out dateModified);

            if (!isCorrectCreationDate || !isCorrectDateModified)
                throw new ImporterException(ImporterResource.WrongFormat_Exception);

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
                throw new ImporterException(exception.Message, exception);
            }
            catch (XmlException exception)
            {
                throw new ImporterException(exception.Message, exception);
            }
        }
    }
}
