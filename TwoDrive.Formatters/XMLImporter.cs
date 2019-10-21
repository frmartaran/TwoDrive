using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Importer.Interface;
using TwoDrive.Importers.Exceptions;
using TwoDrive.Importers.Extensions;
using System.Linq;
using TwoDrive.Importer;

namespace TwoDrive.Importers
{
    public class XMLImporter : ITreeImporter
    {
        public ILogic<Folder> LogicToSave { get; set; }
        public IFileLogic FileLogic { get; set; }
        public string FileExtension { get; set; }
        public Writer WriterFor { get; set; }

        public XMLImporter(ILogic<Folder> logic, IFileLogic fileLogic)
        {
            LogicToSave = logic;
            FileLogic = fileLogic;
        }

        public void Import(string path)
        {
            var document = Load<XmlDocument>(path);
            var rootNode = document.DocumentElement;
            var root = CreateFolder(rootNode, ImporterConstants.Root);
            LogicToSave.Create(root);
            WriterFor.AddRootClaims(root);
            var fileNodes = rootNode.GetElementsByTagName(ImporterConstants.File);
            AddFiles(root, fileNodes);
            AddChildFolders(rootNode, root);

        }

        private void AddFiles(Folder parentFolder, XmlNodeList fileNodes)
        {
            foreach (XmlElement element in fileNodes)
            {
                ValidateDates(element, out DateTime creationDate, out DateTime dateModified);
                var nameAttribute = element.Attributes[ImporterConstants.Name];
                var name = nameAttribute.Value;
                var contentNode = element.GetElementsByTagName(ImporterConstants.Content);
                var typeNode = element.Attributes[ImporterConstants.Type];
                var type = typeNode.Value;
                Domain.FileManagement.File file = new TxtFile();
                switch (type)
                {
                    case "txt":
                        file = new TxtFile
                        {
                            CreationDate = creationDate,
                            DateModified = dateModified,
                            Name = name,
                            Content = contentNode.Item(0).InnerText,
                            Owner = WriterFor,
                            ParentFolder = parentFolder
                        };
                        break;
                    case "html":
                        file = new HTMLFile
                        {
                            CreationDate = creationDate,
                            DateModified = dateModified,
                            Name = name,
                            Content = contentNode.Item(0).InnerText,
                            Owner = WriterFor,
                            ParentFolder = parentFolder,
                            ShouldRender = true
                        };

                        break;
                    default:
                        throw new ImporterException(ImporterResource.Unsupported);
                }

                FileLogic.Create(file);
                WriterFor.AddCreatorClaimsTo(file);
            }
        }

        private void AddChildFolders(XmlElement parentNode, Folder parentFolder)
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
                if (nameAttribute == null)
                    throw new ImporterException(ImporterResource.NoName_Exception);

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
