using System;
using System.Linq;
using TwoDrive.BusinessLogic.Interface;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{
    public abstract class ElementValidator : IElementValidator
    {
        public bool IsValid(Element element)
        {
            ValidateName(element);
            ValidateOwner(element);
            ValidateParentFolder(element);
            ValidateNamesAtSameLevel(element);
            ValidateDates(element);
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

        private void ValidateDates(Element element)
        {
            var earlyModifiedDate = element.CreationDate.CompareTo(element.DateModified);
            if (earlyModifiedDate > 0)
                throw new ArgumentException("The modified date should be later than the creation date");
        }

        public bool ValidateDependenciesAreSet(IFolderRepository folderRepository, IFileRepository fileRepository)
        {
            throw new NotImplementedException();
        }

        public bool IsValidDestination(Element elementToTransfer, Element elementDestination)
        {
            throw new NotImplementedException();
        }
    }

}