using System;

namespace TwoDrive.BusinessLogic.Exceptions
{
    public class LogicException : Exception
    {
        public LogicException(string message) : base(message){}
    }
}