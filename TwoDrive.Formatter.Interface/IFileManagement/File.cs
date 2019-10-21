using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Importer.Interface.IFileManagement
{
    public interface IFile : IElement
    {
        string Extension { get; set; }

        bool ShouldRender { get; set; }

        string Content { get; set; }
    }
}
