using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Helpers.LogicInput
{
    public class ImporterDependencies
    {
        public IFolderLogic FolderLogic { get; set; }

        public IFileLogic FileLogic { get; set; }

        public ILogic<Writer> WriterLogic { get; set; }

        public IModificationLogic ModificationLogic { get; set; }

        public ImporterDependencies(IFolderLogic folderLogic, 
            IFileLogic fileLogic, ILogic<Writer> writerLogic, IModificationLogic modificationLogic)
        {
            FolderLogic = folderLogic;
            FileLogic = fileLogic;
            WriterLogic = writerLogic;
            ModificationLogic = modificationLogic;
        }
    }
}
