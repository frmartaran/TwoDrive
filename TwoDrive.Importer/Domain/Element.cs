using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.Importer.Domain
{
    public abstract class Element : IElement
    {
        public string Name { get; set; }
        public IFolder ParentFolder { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DateModified { get; set; }
    }
}
