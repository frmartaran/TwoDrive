using System;
using System.Collections.Generic;
using System.Linq;
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

        public static bool ElementIsFromOwnerAndIsRootFolder(Element element, int id)
        {
            return element.OwnerId == id && !element.ParentFolderId.HasValue;

        }

        public static bool ElementIsntFromOwner(Element element, int id)
        {
            return element.OwnerId != id;
        }

        public static bool WriterHasClaimsForParent(ICollection<CustomClaim> claims, Element element, int firstElementId)
        {
            if (element != null)
            {
                var result = WriterHasClaimsForParent(claims, element.ParentFolder, firstElementId);
                if (!result)
                {
                    foreach (var claim in claims)
                    {
                        if (claim.Element.Id == element.Id && claim.Element.Id != firstElementId)
                        {
                            result = true;
                            break;
                        }
                    }
                }
                return result;
            }
            else
            {
                return false;
            }
        }
    }
}
