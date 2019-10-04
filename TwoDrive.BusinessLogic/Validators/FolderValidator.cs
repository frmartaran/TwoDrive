using System;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
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
                if(hasSameName)
                    throw new ValidationException("Two folders at same level can have the same name");

            }

        }

        protected override void ValidateParentFolder(Element element)
        {
            var hasParentFolder = element.ParentFolder != null;
            if (!isRoot(element) && !hasParentFolder)
                throw new ValidationException("A child folder must have a parent folder");
        }

        private bool isRoot(Element folder)
        {
            return folder.Name == rootName;

        }
    }
}