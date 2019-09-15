using System;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{
    public class ElementValidator : IValidator<Element>
    {
        public bool isValid(Element element)
        {
            ValidateName(element);
            ValidateOwner(element);

            var hasParentFolder = element.ParentFolder != null;
            var isElementAFolder = element.GetType().Name == "Folder";

            if (!isElementAFolder)
            {
                if (!hasParentFolder)
                    throw new ArgumentException("A file should have a parent folder");
            }
            else
            {
                if (element.Name != "Root" && !hasParentFolder)
                    throw new ArgumentException("A child folder should have a parent folder");
            }

            return true;
        }

        private void ValidateOwner(Element element)
        {
            var hasOwner = element.Owner != null;
            if (!hasOwner)
                throw new ArgumentException("The folder or file must have an owner");
        }

        private void ValidateName(Element element)
        {
            var hasName = !string.IsNullOrWhiteSpace(element.Name);
            if (!hasName)
                throw new ArgumentException("The folder or file should have a name");
        }
    }

}