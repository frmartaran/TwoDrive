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
            ValidateUserName(writer);
            ValidatePassword(writer);
            ValidateToken(writer);
            ValidateClaims(writer);

            return true;
        }

        private static void ValidateClaims(Writer writer)
        {
            ValidateClaimsListNotEmpty(writer);
            ValidateDeleteClaimOverRoot(writer);

            var rootClaims = writer.Claims
            .Where(c => c.Element.Name == "Root")
            .Where(c => c.Element.Owner.Id == writer.Id)
            .ToList();

            var canReadRoot = rootClaims
            .Where(c => c.Type == ClaimType.Read)
            .Any();

            var canwriteRoot = rootClaims
            .Where(c => c.Type == ClaimType.Write)
            .Any();

            var canShareRoot = rootClaims
            .Where(c => c.Type == ClaimType.Share)
            .Any();

            if(!canReadRoot || !canwriteRoot || !canShareRoot)
                throw new ArgumentException("A writer must be able to read/write/share their root");
        }

        private static void ValidateDeleteClaimOverRoot(Writer writer)
        {
            var canDeleteRoot = writer.Claims
                        .Where(c => c.Type == ClaimType.Delete)
                        .Where(c => c.Element.Name == "Root")
                        .Where(c => c.Element.Owner.Id == writer.Id).
                        Any();
            if (canDeleteRoot)
                throw new ArgumentException("A writer can't delete their root folder");
        }

        private static void ValidateClaimsListNotEmpty(Writer writer)
        {
            var isListNull = writer.Claims == null;
            if (isListNull || writer.Claims.Count() == 0)
                throw new ArgumentException("The list of claims is empty");
        }
        private static void ValidateToken(Writer writer)
        {
            var hasToken = writer.Token != Guid.Empty;
            if (!hasToken)
                throw new ArgumentException("Writer has no token");
        }

        private static void ValidatePassword(Writer writer)
        {
            var hasPassword = !string.IsNullOrWhiteSpace(writer.Password);
            if (!hasPassword)
                throw new ArgumentException("Writer has no password");
        }

        private static void ValidateUserName(Writer writer)
        {
            var hasUserName = !string.IsNullOrWhiteSpace(writer.UserName);
            if (!hasUserName)
                throw new ArgumentException("Writer has no username set");
        }
    }
}