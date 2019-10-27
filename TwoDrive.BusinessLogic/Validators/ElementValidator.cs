using System;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Resources;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{
    public abstract class ElementValidator : IValidator<Element>
    {
        public bool IsValid(Element element)
        {
            ValidateIfNull(element);
            ValidateName(element);
            ValidateOwner(element);
            ValidateParentFolder(element);
            ValidateNamesAtSameLevel(element);
            ValidateDates(element);
            Hook(element);

            return true;
        }

        private static void ValidateIfNull(Element element)
        {
            if (element == null)
                throw new ValidationException(BusinessResource.NotNull_Validator);
        }

        protected virtual void Hook(Element element){}

        protected abstract void ValidateNamesAtSameLevel(Element element);

        protected abstract void ValidateParentFolder(Element element);

        private void ValidateOwner(Element element)
        {
            var hasOwner = element.Owner != null;
            if (!hasOwner)
                throw new ValidationException(BusinessResource.MustHaveAnOwner_ElementValidator);
        }

        private void ValidateName(Element element)
        {
            var hasName = !string.IsNullOrWhiteSpace(element.Name);
            if (!hasName)
                throw new ValidationException(BusinessResource.Name_ElementValidator);
        }

        private void ValidateDates(Element element)
        {
            var earlyModifiedDate = element.CreationDate.CompareTo(element.DateModified);
            if (earlyModifiedDate > 0)
                throw new ValidationException(BusinessResource.Date_ElementValidator);
        }
    }
}