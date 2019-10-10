using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic
{
    public abstract class ElementLogic : IElementLogic
    {
        public virtual void MoveElement(Element elementToMove, Folder folderDestination, MoveElementDependencies dependencies)
        {
            try
            {
                if (dependencies.ElementValidator.IsValidDestination(elementToMove, folderDestination))
                {
                    elementToMove.ParentFolder.FolderChildren = elementToMove.ParentFolder.FolderChildren
                        .Where(fc => fc.Id != elementToMove.Id)
                        .ToList();
                    elementToMove.ParentFolder = folderDestination;
                    elementToMove.ParentFolderId = folderDestination.Id;
                    dependencies.ElementRepository.Update(elementToMove.ParentFolder);
                    dependencies.ElementRepository.Update(elementToMove);
                    dependencies.ElementRepository.Save();
                }
            }
            catch(LogicException exception)
            {
                throw new LogicException(exception.Message, exception);
            }

        }
    }
}