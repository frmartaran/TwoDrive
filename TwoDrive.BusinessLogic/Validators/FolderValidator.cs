using System;
using System.Linq;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{

    public class FolderValidator : ElementValidator
    {
        private const string rootName = "Root";
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

        public bool IsValidDestination(Writer ownerOfFolderToTransfer, Element elementDestination)
        {
            ValidateDestinationIsAFolder();

            return true;
        }

        private void ValidateDestinationIsAFolder()
        {
            throw new NotImplementedException();
        }

        private bool isRoot(Element folder)
        {
            return folder.Name == rootName;
        }
    }
}