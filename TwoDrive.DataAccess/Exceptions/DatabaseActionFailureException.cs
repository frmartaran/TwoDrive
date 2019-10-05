using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.DataAccess.Exceptions
{
    public class DatabaseActionFailureException : Exception
    {
        public DatabaseActionFailureException(string message, Exception innerException): base(message, innerException) { }
    }
}
