﻿using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface IImporterLogic
    {
        ImportingOptions Options { get; set; }
        IImporter GetImporter(string path);

        void Import(string path);

        List<ImporterInfo> GetAllImporters(string path);
    }
}
