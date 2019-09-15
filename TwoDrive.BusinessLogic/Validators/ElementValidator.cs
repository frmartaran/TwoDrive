using System;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{
    public class ElementValidator : IValidator<Element>
    {
        public bool isValid(Element element)
        {
            var hasName = !string.IsNullOrWhiteSpace(element.Name);
            if (!hasName)
                throw new ArgumentException("The folder or file should have a name");
                
            return true;
        }
    }

}