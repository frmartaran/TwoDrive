using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface;

namespace TwoDrive.BusinessLogic.Interfaces.LogicInput
{
    public class ImporterInfo
    {
        public string Name { get; set; }

        public ParameterDictionary Parameters { get; set; }
    }
}
