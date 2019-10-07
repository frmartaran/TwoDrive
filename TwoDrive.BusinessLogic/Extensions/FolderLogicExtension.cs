using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Extensions
{
    public static class FolderLogicExtension
    {
        public static bool IsOwnerOfOriginAndDestination(this Writer writer, Folder folderToMove, Folder folderDestination)
        {
            return writer.Id == folderToMove.Owner.Id  && writer.Id == folderDestination.Owner.Id;
        }
    }
}
