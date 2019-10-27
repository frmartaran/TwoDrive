using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic.Helpers.LogicInput
{
    public class ImporterLogicDependencies
    {
        public IFolderLogic FolderLogic { get; set; }

        public IFileLogic FileLogic { get; set; }

        public ILogic<Writer> WriterLogic { get; set; }

        public ImporterLogicDependencies(IFolderLogic folderLogic, 
            IFileLogic fileLogic, ILogic<Writer> writerLogic)
        {
            FolderLogic = folderLogic;
            FileLogic = fileLogic;
            WriterLogic = writerLogic;
        }
    }
}
