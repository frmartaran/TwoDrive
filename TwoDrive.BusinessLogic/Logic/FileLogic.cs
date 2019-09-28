using System;
using System.Collections.Generic;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic
{
    public class FileLogic : ILogic<File>, IFileLogic
    {
        private IRepository<File> repository;
        
        public FileLogic(IRepository<File> repository)
        {
            this.repository = repository;
        }

        public void Create(File objectToCreate)
        {
            repository.Insert(objectToCreate);
        }

        public void Delete(File objectToCreate)
        {
            throw new NotImplementedException();
        }

        public File Get(int Id)
        {
            throw new NotImplementedException();
        }

        public ICollection<File> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(File objectToCreate)
        {
            throw new NotImplementedException();
        }
    }
}
