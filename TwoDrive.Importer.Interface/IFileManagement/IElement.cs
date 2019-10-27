using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Importer.Interface.IFileManagement
{
    public interface IElement
    {
        string Name { get; set; }
        IFolder ParentFolder { get; set; }
        DateTime CreationDate { get; set; }
        DateTime DateModified { get; set; }
    }
}
