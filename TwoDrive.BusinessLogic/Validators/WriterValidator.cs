using System;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic.Validators
{
    public class WriterValidator : IValidator<Writer>
    {
        public bool isValid(Writer writer)
        {

            return writer.Token != Guid.Empty && !string.IsNullOrWhiteSpace(writer.UserName);
        }
    }
}