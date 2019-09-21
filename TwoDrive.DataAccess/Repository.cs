using System.Security;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TwoDrive.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected TwoDriveDbContext context;
        protected DbSet<T> table;

        public Repository(TwoDriveDbContext current)
        {
            context = current;
            table = context.Set<T>();
        }
        public void Insert(T objectToCreate)
        {
            table.Add(objectToCreate);
        }

        public virtual T Get(int Id)
        {
            return table.Find(Id);
        }

        public void Update(T objectToUpdate)
        {
            table.Attach(objectToUpdate);
            var entry = context.Entry(objectToUpdate);
            SetModified(entry);
        }

        public void Delete(int Id)
        {
            var existingObject = table.Find(Id);
            table.Remove(existingObject);
        }

        public ICollection<T> GetAll()
        {
            return table.ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private void SetModified(EntityEntry<T> entity)
        {
            if (entity != null)
            {
                entity.State = EntityState.Modified;
            }
        }
        public virtual bool Exists(T objectToFind)
        {
            return table.Any();
        }
    }
}