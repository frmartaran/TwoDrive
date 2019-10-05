using System.Linq;
using TwoDrive.BusinessLogic.Interface;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic
{
    public class ElementLogic : IElementLogic
    {
        private IFolderRepository FolderRepository { get; set; }

        private IElementValidator ElementValidator { get; set; }

        private IFileRepository FileRepository { get; set; }

        public ElementLogic() { }

        public ElementLogic(ElementLogicDependencies dependencies)
        {
            FolderRepository = dependencies.FolderRepository;
            ElementValidator = dependencies.ElementValidator;
            FileRepository = dependencies.FileRepository;
        }

        public virtual void MoveElement(Element elementToMove, Element elementDestination)
        {
            ElementValidator.ValidateDependenciesAreSet(FolderRepository, FileRepository);
            if (ElementValidator.IsValidDestination(elementToMove, elementDestination))
            {
                var folderDestination = (Folder)elementDestination;
                elementToMove.ParentFolder.FolderChildren = elementToMove.ParentFolder.FolderChildren.Where(fc => fc.Id != elementToMove.Id)
                    .ToList();
                elementToMove.ParentFolder = folderDestination;
                elementToMove.ParentFolderId = folderDestination.Id;
                FolderRepository.Update(elementToMove.ParentFolder);
                if (elementDestination is Folder folder)
                {
                    FolderRepository.Update(folder);
                }
                else if (elementDestination is File file)
                {
                    FileRepository.Update(file);
                }
                FolderRepository.Save();
                FileRepository.Save();
            }
        }
    }
}