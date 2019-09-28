using System.Collections.Generic;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface ILogic<T>
    where T : class
    {
        void Create(T objectToCreate);
        void Update(T objectToUpdate);
        void Delete(int id);
        T Get(int Id);
        ICollection<T> GetAll();
    }

}