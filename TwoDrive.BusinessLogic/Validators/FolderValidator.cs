using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Resources;
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
                    .OfType<Folder>()
                    .Where(f => f.Name == element.Name)
                    .Where(f => f.Id != element.Id)
                    .Any();
                if (hasSameName)
                    throw new ValidationException(BusinessResource.SameName_FolderValidator);
            }
        }

        protected override void ValidateParentFolder(Element element)
        {
            var hasParentFolder = element.ParentFolder != null;
            if (!IsRoot(element) && !hasParentFolder)
                throw new ValidationException(BusinessResource.MissingParent_FolderValidator);
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
                throw new ValidationException(BusinessResource.MissingDependencies_FolderValidator);
            }
        }

        private void ValidateDestinationIsMyRootChild(int ownerId, Folder folderDestination)
        {
            var ownerRootFolder = FolderRepository.GetRoot(ownerId);
            var isDestinationMyRootChild = IsElementInsideFolder(ownerRootFolder, folderDestination);
            if (!isDestinationMyRootChild)
            {
                throw new ValidationException(BusinessResource.DestinationNotInRoot_FolderValidator);
            }
        }

        private void ValidateDestinationExists(Folder folderDestination)
        {
            var isDestinationInDB = FolderRepository.Get(folderDestination.Id) != null;
            if (!isDestinationInDB)
            {
                throw new ValidationException(BusinessResource.DestinationNotFound_FolderValidator);
            }
        }

        private void ValidateDestinationIsNotChildOfElementToTransfer(Element elementToTransfer, Folder folderDestination)
        {
            if (elementToTransfer is Folder folder)
            {
                if (IsElementInsideFolder(folder, folderDestination))
                {
                    throw new ValidationException(BusinessResource.ChildDestination_FolderValidator);
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
            if (containerFolder.FolderChildren.Count > 0)
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

        protected override void Hook(Element element)
        {
            var rootAlreadyExists = FolderRepository.GetAll()
                .Where(f => f.Owner == element.Owner)
                .Any();
            if (IsRoot(element) && rootAlreadyExists)
                throw new ValidationException(BusinessResource.RootAlreadyExists);
        }
    }
}