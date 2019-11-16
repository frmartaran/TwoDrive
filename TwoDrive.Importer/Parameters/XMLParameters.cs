﻿using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface;
using TwoDrive.Importers;

namespace TwoDrive.Importer.Parameters
{
    public class XMLParameters : ImportingParameters
    {
        public XMLParameters()
        {
            ParentImporter = new XMLImporter();
        }

        public string Path { get; set; }
    }
}
