using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Extensions
{
    public static class FolderLogicExtension
    {
        public static bool IsOwnerOfOriginAndDestination(this Writer writer, Element elementToMove, Folder folderDestination)
        {
            if(elementToMove != null && folderDestination != null)
            {
                return writer.Id == elementToMove.Owner.Id && writer.Id == folderDestination.Owner.Id;
            }
            else
            {
                return false;
            }
        }
    }
}
