﻿using System;
using System.Xml;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Formatter.Interface;

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
        }

        public T Load<T> (string path) where T : class
        {
            var document = new XmlDocument();
            document.Load(path);
            return document as T;
        }
    }
}
