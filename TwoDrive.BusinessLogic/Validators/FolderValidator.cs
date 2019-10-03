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

        private IRepository<Folder> FolderRepository { get; set; }

        private IRepository<File> FileRepository { get; set; }

        public FolderValidator() { }

        public FolderValidator(IRepository<Folder> FolderRepository, IRepository<File> FileRepository)
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

        public bool IsValidDestination(Writer ownerOfFolderToTransfer, Element elementDestination)
        {
            var folderDestination = ValidateDestinationIsAFolder(elementDestination);
            return true;
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

        private bool isRoot(Element folder)
        {
            return folder.Name == rootName;
        }
    }
}