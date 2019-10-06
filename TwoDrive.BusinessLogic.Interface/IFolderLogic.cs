﻿using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface IFolderLogic : ILogic<Folder>
    {
        Folder GetRootFolder(Writer owner);

        string ShowTree(Folder folder);
    }
}