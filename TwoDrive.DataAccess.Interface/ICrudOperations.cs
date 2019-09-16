using System;
using System.Collections.Generic;
using TwoDrive.Domain;

namespace TwoDrive.DataAccess.Interface
{
    public interface ICrudOperations<T> where T : class
    {
        void Create(T objectToCreate);
        T Read(int Id);
        void Update(T objectToUpdate);
        void Delete(int Id);
        ICollection<T> ReadAll();
        void Save();
    }
}
