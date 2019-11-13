using System;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Resources;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{
    public class FileValidator : ElementValidator
    {
        protected override void ValidateNamesAtSameLevel(Element element)
        {
            var ParentFolder = element.ParentFolder;
            if(ParentFolder.FolderChildren != null)
            {
                var hasSameName = ParentFolder.FolderChildren
                    .Where(f => f.Name == element.Name)
                    .Where(f => f.Id != element.Id)
                    .Where(f => f.GetType() == element.GetType())
                    .Any();
                if (hasSameName)
                    throw new ValidationException(BusinessResource.SameName_FileValidator);
            }
        }

        protected override void ValidateParentFolder(Element element)
        {
            var hasParentFolder = element.ParentFolder != null;
            if (!hasParentFolder)
                throw new ValidationException(BusinessResource.MissingParent_FileValidator);
        }
    }
}