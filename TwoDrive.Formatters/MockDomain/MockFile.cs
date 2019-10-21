﻿using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.Importer.MockDomain
{
    public class MockFile : MockElement, IFile
    {
        public string Extension { get; set; }
        public string ShouldRender { get; set; }
        public string Content { get; set; }
    }
}
