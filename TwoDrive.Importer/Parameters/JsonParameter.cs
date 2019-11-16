using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface;

namespace TwoDrive.Importer.Parameters
{
    public class JsonParameter : ImportingParameters
    {
        public JsonParameter()
        {
            ParentImporter = new JsonImporter();
        }

        public string Path { get; set; }


    }
}
