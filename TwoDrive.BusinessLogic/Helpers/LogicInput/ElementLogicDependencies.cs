using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Interfaces.LogicInput
{
    public class ElementLogicDependencies
    {
        public IFolderRepository FolderRepository { get; set; }

        public IFileRepository FileRepository { get; set; }

        public IElementValidator ElementValidator { get; set; }

        public IRepository<Modification> ModificationRepository { get; set; }
    }
}