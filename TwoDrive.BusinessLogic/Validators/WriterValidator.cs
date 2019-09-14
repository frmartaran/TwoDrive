using System;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic.Validators
{
    public class WriterValidator : IValidator<Writer>
    {
        public bool isValid(Writer writer)
        {
            var hasUserName = !string.IsNullOrWhiteSpace(writer.UserName);
            if(!hasUserName)
                throw new ArgumentException("Writer has no username set");

            var hasToken = writer.Token != Guid.Empty;
            if(!hasToken)
                throw new ArgumentException("Writer has no token");

            return true;
        }
    }
}