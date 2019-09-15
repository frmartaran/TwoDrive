using System;
using System.Linq;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{
    public abstract class ElementValidator : IValidator<Element>
    {
        public bool isValid(Element element)
        {
            ValidateName(element);
            ValidateOwner(element);
            ValidateParentFolder(element);
            ValidateNamesAtSameLevel(element);
            Hook(element);

            return true;
        }

        protected virtual void Hook(Element element){}

        protected abstract void ValidateNamesAtSameLevel(Element element);

        protected abstract void ValidateParentFolder(Element element);

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