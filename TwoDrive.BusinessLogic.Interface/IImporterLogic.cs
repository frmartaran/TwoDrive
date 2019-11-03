using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface IImporterLogic
    {
        ImportingOptions Options { get; set; }
        IImporter<IFolder> GetImporter();

        void Import();

        List<string> GetAllImporters();
    }
}
