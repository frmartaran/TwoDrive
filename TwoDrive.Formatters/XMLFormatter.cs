﻿using System;
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
            ValidateDates(node, out DateTime creationDate, out DateTime dateModified);
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