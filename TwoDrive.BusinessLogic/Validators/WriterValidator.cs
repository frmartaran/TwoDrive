using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic.Validators
{
    public class WriterValidator : IValidator<Writer>
    {
        private IRepository<Writer> repository { get; set; }

        public WriterValidator(IRepository<Writer> current)
        {
            repository = current;
        }

        public bool IsValid(Writer writer)
        {
            ValidateIfNull(writer);
            ValidateUserName(writer);
            ValidatePassword(writer);
            return true;
        }

        private void ValidateIfNull(Writer writer)
        {
            if (writer == null)
                throw new ValidationException("Can't add or update null value");
        }
        
        private void ValidatePassword(Writer writer)
        {
            var hasPassword = !string.IsNullOrWhiteSpace(writer.Password);
            if (!hasPassword)
                throw new ValidationException("Writer has no password");
        }
        private void ValidateUserName(Writer writer)
        {
            var hasUserName = !string.IsNullOrWhiteSpace(writer.UserName);
            if (!hasUserName)
                throw new ValidationException("Writer has no username set");

            var usernameExists = repository.Exists(writer);
            if (usernameExists)
                throw new ValidationException("The username must be unique");
        }
    }
}