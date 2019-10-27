using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Importer.Interface.IFileManagement
{
    public interface IFolder : IElement
    {
        List<IElement> FolderChildren { get; set; }
    }
}
