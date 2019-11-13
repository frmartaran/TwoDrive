using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Resources;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic.Validators
{
    public class WriterValidator : IValidator<Writer>
    {
        private IRepository<Writer> Repository { get; set; }

        public WriterValidator(IRepository<Writer> current)
        {
            Repository = current;
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
                throw new ValidationException(BusinessResource.NotNull_Validator);
        }
        
        private void ValidatePassword(Writer writer)
        {
            var hasPassword = !string.IsNullOrWhiteSpace(writer.Password);
            if (!hasPassword)
                throw new ValidationException(BusinessResource.MissingPassword_WriterValidator);
        }
        private void ValidateUserName(Writer writer)
        {
            var hasUserName = !string.IsNullOrWhiteSpace(writer.UserName);
            if (!hasUserName)
                throw new ValidationException(BusinessResource.MissingUserName_WriterValidator);

            var usernameExists = Repository.Exists(writer);
            if (usernameExists)
                throw new ValidationException(BusinessResource.NotUnique_WriterValidator);
        }
    }
}