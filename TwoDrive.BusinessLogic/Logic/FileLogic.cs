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

        private IValidator<Element> ElementValidator;

        public FileLogic(IRepository<File> repository)
        {
            this.FileRepository = repository;
        }

        public FileLogic(IRepository<File> repository, IValidator<Element> validator)
        {
            this.FileRepository = repository;
            this.ElementValidator = validator;
        }

        public void Create(File fileToCreate)
        {
            ElementValidator.isValid(fileToCreate);
            FileRepository.Insert(fileToCreate);
            FileRepository.Save();
        }

        public void Delete(int id)
        {
            FileRepository.Delete(id);
            FileRepository.Save();
        }

        public File Get(int Id)
        {
            return FileRepository.Get(Id);
        }

        public ICollection<File> GetAll()
        {
            return FileRepository.GetAll();
        }

        public void Update(File fileToUpdate)
        {
            ElementValidator.isValid(fileToUpdate);
            fileToUpdate.DateModified = DateTime.Now;
            FileRepository.Update(fileToUpdate);
            FileRepository.Save();
        }
    }
}
