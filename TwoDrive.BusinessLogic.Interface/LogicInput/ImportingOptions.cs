using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Domain;
using TwoDrive.Importer.Interface;

namespace TwoDrive.BusinessLogic.Helpers.LogicInput
{
    public class ImportingOptions
    {
        public Writer Owner { get; set; }

        public string FileType { get; set; }

        public string FilePath { get; set; }

        public ImportingParameters Parameters { get; set; }
    }
}
