using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.Importer.Domain
{
    public class File : Element, IFile
    {
        public string Extension { get; set; }
        public bool ShouldRender { get; set; }
        public string Content { get; set; }
    }
}
