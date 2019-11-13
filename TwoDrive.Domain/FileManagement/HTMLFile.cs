using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.Domain.FileManagement
{
    public class HTMLFile : File
    {
        public bool ShouldRender { get; set; }
    }
}
