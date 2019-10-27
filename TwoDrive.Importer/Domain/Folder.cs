using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.Importer.Domain
{
    public class Folder : Element, IFolder
    {
        public List<IElement> FolderChildren { get; set; }
    }
}
