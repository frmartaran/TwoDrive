using System;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Validators
{
    public class ElementValidator : IValidator<Element>
    {
        public bool isValid(Element objectToValidate)
        {
            return true;
        }
    }

}