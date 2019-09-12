using System;

namespace TwoDrive.Domain.FileManagement
{
    public abstract class File : Element
    {
        public DateTime CreationDate { get; set; }

        public DateTime DateModified { get; set; }
    }
}