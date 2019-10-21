using System;

namespace TwoDrive.Domain.FileManagement
{
    public abstract class File : Element
    {
        public string Extension { get; set; }
        public string Content { get; set; }
    }
}