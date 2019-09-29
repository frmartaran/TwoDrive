using System;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface IValidator<T>
    where T : class
    {
        bool IsValid(T objectToValidate);
    }
}
