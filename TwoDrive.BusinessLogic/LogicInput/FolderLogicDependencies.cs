using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.LogicInput
{
    public class FolderLogicDependencies
    {
        public IFolderRepository FolderRepository { get; set; }

        public IFileRepository FileRepository { get; set; }

        public IValidator<Element> ElementValidator { get; set; }
    }
}