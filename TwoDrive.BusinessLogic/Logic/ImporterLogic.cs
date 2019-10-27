using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.BusinessLogic.Logic
{
    public class ImporterLogic
    {
        private IFolderLogic FolderLogic { get; set; }

        private IFileLogic FileLogic { get; set; }

        private ILogic<Writer> WriterLogic { get; set; }

        private ImportingOptions Options { get; set; }

        public ImporterLogic(ImportingOptions options, ImporterLogicDependencies dependencies)
        {
            Options = options;
            FolderLogic = dependencies.FolderLogic;
            FileLogic = dependencies.FileLogic;
            WriterLogic = dependencies.WriterLogic;
        }

        public IImporter<IFolder> GetImporter()
        {
            var assemblyInfo = Assembly.LoadFrom("TwoDrive.Importer.dll");

            return null;
        }
    }
}
