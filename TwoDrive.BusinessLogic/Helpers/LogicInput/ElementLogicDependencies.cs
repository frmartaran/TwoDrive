using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Validators;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Interfaces.LogicInput
{
    public class ElementLogicDependencies
    {
        public IFolderRepository FolderRepository { get; set; }

        public IFileRepository FileRepository { get; set; }

        public IFolderValidator ElementValidator { get; set; }

        public IRepository<Modification> ModificationRepository { get; set; }

        public ElementLogicDependencies(IFolderRepository folderRepository,
            IFileRepository fileRepository, IFolderValidator folderValidator,
            IRepository<Modification> repository)
        {

            FolderRepository = folderRepository;
            FileRepository = fileRepository;
            ElementValidator = folderValidator;
            ModificationRepository = repository;

        }
    }
}