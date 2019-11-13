
using System;

namespace TwoDrive.BusinessLogic.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
}