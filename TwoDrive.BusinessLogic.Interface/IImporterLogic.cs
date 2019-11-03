using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface IImporterLogic
    {
        IImporter<IFolder> GetImporter();

        void Import();

        List<string> GetAllImporters();
    }
}
