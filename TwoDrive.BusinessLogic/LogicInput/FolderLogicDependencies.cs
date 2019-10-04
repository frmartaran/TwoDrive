using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.LogicInput
{
    public class FolderLogicDependencies
    {
        public IRepository<Folder> FolderRepository { get; set; }

        public IRepository<File> FileRepository { get; set; }

        public IValidator<Element> ElementValidator { get; set; }

        public IRepository<Modification> ModificationRepository { get; set; }
    }
}