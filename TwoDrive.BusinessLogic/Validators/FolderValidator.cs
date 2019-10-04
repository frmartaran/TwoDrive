using System;
using System.Linq;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{

    public class FolderValidator : ElementValidator
    {
        private const string rootName = "Root";

        private IFolderRepository FolderRepository { get; set; }

        private IFileRepository FileRepository { get; set; }

        public FolderValidator() { }

        public FolderValidator(IFolderRepository FolderRepository, IFileRepository FileRepository)
        {
            this.FolderRepository = FolderRepository;
            this.FileRepository = FileRepository;
        }

        protected override void ValidateNamesAtSameLevel(Element element)
        {
            if (!isRoot(element))
            {
                var ParentFolder = element.ParentFolder;
                var hasSameName = ParentFolder.FolderChildren
                .Where(f => f.Name == element.Name)
                .Where(f => f.GetType().Name == "Folder")
                .Any();
                if (hasSameName)
                    throw new ArgumentException("Two folders at same level can have the same name");
            }

        }

        protected override void ValidateParentFolder(Element element)
        {
            var hasParentFolder = element.ParentFolder != null;
            if (!isRoot(element) && !hasParentFolder)
                throw new ArgumentException("A child folder must have a parent folder");
        }

        public bool IsValidDestination(Element elementToTransfer, Element elementDestination)
        {
            if (!AreElementToTransferAndDestinationEmpty(elementToTransfer, elementDestination) && AreDependenciesSet())
            {
                var folderDestination = ValidateDestinationIsAFolder(elementDestination);
                ValidateDestinationExists(folderDestination);
                ValidateDestinationIsMyRootChild(elementToTransfer.Owner.Id, folderDestination);
                ValidateDestinationIsNotChildOfElementToTransfer(elementToTransfer, folderDestination);
                return true;
            }
            else
            {
                throw new ArgumentException("Dependencies must be set to validate destination");
            }
        }

        private void ValidateDestinationIsMyRootChild(int ownerId, Folder folderDestination)
        {
            var ownerRootFolder = FolderRepository.GetRoot(ownerId);
            var isDestinationMyRootChild = IsElementInsideFolder(ownerRootFolder, folderDestination);
            if (!isDestinationMyRootChild)
            {
                throw new ArgumentException("Destination must be my root's child");
            }
        }

        private Folder ValidateDestinationIsAFolder(Element elementDestination)
        {
            if (elementDestination is Folder folder)
            {
                return folder;
            }
            else
            {
                throw new ArgumentException("Destination must be a folder");
            }
        }

        private void ValidateDestinationExists(Folder folderDestination)
        {
            var isDestinationInDB = FolderRepository.Get(folderDestination.Id) != null;
            if (!isDestinationInDB)
            {
                throw new ArgumentException("Destination doesnt exists");
            }
        }

        private void ValidateDestinationIsNotChildOfElementToTransfer(Element elementToTransfer, Folder folderDestination)
        {
            if (elementToTransfer is Folder folder)
            {
                if (IsElementInsideFolder(folder, folderDestination))
                {
                    throw new ArgumentException("Destination is child of element to transfer");
                }
            }
        }

        private bool IsElementInsideFolder(Folder containerFolder, Element elementInFolder)
        {
            var result = false;
            if (containerFolder.Id == elementInFolder.Id)
            {
                result = true;
            }
            if (!result)
            {
                foreach (var child in containerFolder.FolderChildren)
                {
                    if (child is Folder folder)
                    {
                        result = IsElementInsideFolder(folder, elementInFolder);
                    }
                }
            }
            return result;
        }

        private bool AreElementToTransferAndDestinationEmpty(Element elementToTransfer, Element elementDestination)
        {
            return elementToTransfer == null && elementDestination == null; 
        }

        private bool isRoot(Element folder)
        {
            return folder.Name == rootName;
        }

        private bool AreDependenciesSet()
        {
            return FolderRepository != null && FileRepository != null;
        }
    }
}