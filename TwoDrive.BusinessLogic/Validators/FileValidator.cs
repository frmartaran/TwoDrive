using System;
using System.Linq;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{
    public class FileValidator : ElementValidator
    {
        protected override void ValidateNamesAtSameLevel(Element element)
        {
            var ParentFolder = element.ParentFolder;
            if (!IsEmpty(ParentFolder))
            {
                var hasSameName = ParentFolder.FolderChilden
                    .Where(f => f.Name == element.Name)
                    .Where(f => f.GetType() == element.GetType())
                    .Any();
                if (hasSameName)
                    throw new ArgumentException("Two files at same level can have the same name");
            }
        }

        private static bool IsEmpty(Folder ParentFolder)
        {
            return ParentFolder.FolderChilden == null || ParentFolder.FolderChilden.Count == 0;
        }

        protected override void ValidateParentFolder(Element element)
        {
            var hasParentFolder = element.ParentFolder != null;
            if (!hasParentFolder)
                throw new ArgumentException("A file should have a parent folder");
        }
    }
}