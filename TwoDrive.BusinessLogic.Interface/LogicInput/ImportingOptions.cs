using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Domain;
using TwoDrive.Importer.Interface;

namespace TwoDrive.BusinessLogic.Interfaces.LogicInput
{
    public class ImportingOptions
    {
        public Writer Owner { get; set; }

        public string ImporterName { get; set; }

        public ImportingParameters Parameters { get; set; }
    }
}
