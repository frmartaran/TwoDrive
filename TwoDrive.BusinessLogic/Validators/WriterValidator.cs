using System;
using System.Linq;
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

            var hasPassword = !string.IsNullOrWhiteSpace(writer.Password);
            if(!hasPassword)
                throw new ArgumentException("Writer has no password");

            var hasToken = writer.Token != Guid.Empty;
            if(!hasToken)
                throw new ArgumentException("Writer has no token");

            var isListNull = writer.Claims == null;
            if(isListNull)
                throw new ArgumentException("The list of claims is empty");
                
            var hasClaims = writer.Claims.Count() != 0;
            if(!hasClaims)
                throw new ArgumentException("The writer should have claims for their root folder");

            return true;
        }
    }
}