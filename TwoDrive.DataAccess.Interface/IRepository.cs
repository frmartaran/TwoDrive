using System;
using System.Collections.Generic;
using TwoDrive.Domain;

namespace TwoDrive.DataAccess.Interface
{
    public interface IRepository<T> where T : class
    {
        void Insert(T objectToCreate);
        T Get(int Id);
        void Update(T objectToUpdate);
        void Delete(int Id);
        ICollection<T> GetAll();
        void Save();
    }
}
