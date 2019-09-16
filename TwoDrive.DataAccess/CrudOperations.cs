using System.Security;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TwoDrive.DataAccess
{
    public class CrudOperations<T> : ICrudOperations<T> where T : class
    {
        private TwoDriveDbContext context;
        private DbSet<T> table;

        public CrudOperations(TwoDriveDbContext context)
        {
            this.context = context;
            table = context.Set<T>();
        }

        public void Create(T objectToCreate)
        {
            table.Add(objectToCreate);
        }

        public T Read(int Id)
        {
            return table.Find(Id);
        }

        public void Delete(int Id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<T> ReadAll()
        {
            return table.ToList();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void Update(T objectToUpdate)
        {
            throw new System.NotImplementedException();
        }
    }
}