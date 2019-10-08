using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{

    public class FolderValidator : ElementValidator, IFolderValidator
    {
        private const string rootName = "Root";

        private IFolderRepository FolderRepository { get; set; }

        public FolderValidator(IFolderRepository FolderRepository)
        {
            this.FolderRepository = FolderRepository;
        }

        protected override void ValidateNamesAtSameLevel(Element element)
        {
            if (!IsRoot(element))
            {
                var ParentFolder = element.ParentFolder;
                var hasSameName = ParentFolder.FolderChildren
                .Where(f => f.Name == element.Name)
                .Where(f => f.GetType().Name == "Folder")
                .Any();
                if(hasSameName)
                    throw new ValidationException("Two folders at same level can have the same name");
            }
        }

        protected override void ValidateParentFolder(Element element)
        {
            var hasParentFolder = element.ParentFolder != null;
            if (!IsRoot(element) && !hasParentFolder)
                throw new ValidationException("A child folder must have a parent folder");
        }

        public bool IsValidDestination(Element elementToTransfer, Folder folderDestination)
        {
            if (!AreElementToTransferAndDestinationEmpty(elementToTransfer, folderDestination))
            {
                ValidateDestinationExists(folderDestination);
                ValidateDestinationIsMyRootChild(elementToTransfer.Owner.Id, folderDestination);
                ValidateDestinationIsNotChildOfElementToTransfer(elementToTransfer, folderDestination);
                return true;
            }
            else
            {
                throw new ValidationException("Dependencies must be set to validate destination");
            }
        }

        private void ValidateDestinationIsMyRootChild(int ownerId, Folder folderDestination)
        {
            var ownerRootFolder = FolderRepository.GetRoot(ownerId);
            var isDestinationMyRootChild = IsElementInsideFolder(ownerRootFolder, folderDestination);
            if (!isDestinationMyRootChild)
            {
                throw new ValidationException("Destination must be my root's child");
            }
        }

        private void ValidateDestinationExists(Folder folderDestination)
        {
            var isDestinationInDB = FolderRepository.Get(folderDestination.Id) != null;
            if (!isDestinationInDB)
            {
                throw new ValidationException("Destination doesnt exists");
            }
        }

        private void ValidateDestinationIsNotChildOfElementToTransfer(Element elementToTransfer, Folder folderDestination)
        {
            if (elementToTransfer is Folder folder)
            {
                if (IsElementInsideFolder(folder, folderDestination))
                {
                    throw new ValidationException("Destination is child of element to transfer");
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
                containerFolder.FolderChildren = GetChildren(containerFolder);
                foreach (var child in containerFolder.FolderChildren)
                {
                    if (child is Folder folder)
                    {
                        result = IsElementInsideFolder(folder, elementInFolder);
                    }
                    if (result)
                        break;
                }
            }
            return result;
        }

        private ICollection<Element> GetChildren(Folder containerFolder)
        {
            if(containerFolder.FolderChildren.Count > 0)
            {
                return containerFolder.FolderChildren;
            }
            else
            {
                return FolderRepository.GetChildren(containerFolder.Id);
            }
        }

        private bool AreElementToTransferAndDestinationEmpty(Element elementToTransfer, Element elementDestination)
        {
            return elementToTransfer == null && elementDestination == null; 
        }

        private bool IsRoot(Element folder)
        {
            return folder.Name == rootName;
        }
    }
}