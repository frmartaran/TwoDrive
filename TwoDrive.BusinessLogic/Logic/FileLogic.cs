using System;
using System.Collections.Generic;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic
{
    public class FileLogic : ILogic<File>, IFileLogic
    {
        private IRepository<File> FileRepository;

        private IValidator<File> FileValidator;

        public FileLogic(IRepository<File> repository, IValidator<File> validator)
        {
            this.FileRepository = repository;
            this.FileValidator = validator;
        }

        public void Create(File fileToCreate)
        {
            FileValidator.isValid(fileToCreate);
            FileRepository.Insert(fileToCreate);
            FileRepository.Save();
        }

        public void Delete(int id)
        {
            FileRepository.Delete(id);
        }

        public File Get(int Id)
        {
            throw new NotImplementedException();
        }

        public ICollection<File> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(File fileToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
