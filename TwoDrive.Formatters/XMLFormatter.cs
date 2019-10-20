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
            var creationDateNode = rootNode.GetElementsByTagName("CreationDate");
            var creationDateString = creationDateNode.Item(0).Value;
            DateTime.TryParse(creationDateString, out DateTime creationDate);

            var dateModifiedNode = rootNode.GetElementsByTagName("DateModified");
            var dateModifiedString = dateModifiedNode.Item(0).Value;
            DateTime.TryParse(dateModifiedString, out DateTime dateModified);

            var root = new Folder
            {
                Owner = WriterFor,
                Name = "Root",
                CreationDate = creationDate,
                DateModified = dateModified,
                FolderChildren = new List<Element>()
            };

            WriterFor.AddRootClaims(root);
            LogicToSave.Create(root);

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
