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

        public virtual void MoveElement(Element elementToMove, Folder elementDestination)
        {
            throw new System.NotImplementedException();
        }
    }
}