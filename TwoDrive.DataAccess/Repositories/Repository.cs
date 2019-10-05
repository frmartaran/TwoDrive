using System.Security;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TwoDrive.DataAccess.Interface;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using TwoDrive.DataAccess.Exceptions;

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
            try
            {
                table.Add(objectToCreate);
            }
            catch (ArgumentNullException exception)
            {
                throw new DatabaseActionFailureException(exception.Message, exception);
            }
        }

        public virtual T Get(int Id)
        {
            try
            {
                return table.Find(Id);

            }
            catch (ArgumentNullException exception)
            {
                throw new DatabaseActionFailureException(exception.Message, exception);

            }
        }

        public void Update(T objectToUpdate)
        {
            try
            {
                table.Attach(objectToUpdate);
                var entry = context.Entry(objectToUpdate);
                SetModified(entry);
            }
            catch (ArgumentNullException exception)
            {
                throw new DatabaseActionFailureException(exception.Message, exception);

            }
        }

        public void Delete(int Id)
        {
            try
            {
                var existingObject = table.Find(Id);
                table.Remove(existingObject);
            }
            catch (ArgumentNullException exception)
            {
                throw new DatabaseActionFailureException(exception.Message, exception);
            }
        }

        public virtual ICollection<T> GetAll()
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