using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.Importer.MockDomain
{
    public class MockFolder : MockElement, IFolder
    {
        public List<IElement> FolderChildren { get; set; }
    }
}
