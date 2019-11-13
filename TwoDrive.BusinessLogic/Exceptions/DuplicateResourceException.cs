using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.BusinessLogic.Exceptions
{
    public class DuplicateResourceException : ValidationException
    {
        public DuplicateResourceException(string message) : base(message) { }
    }
}
