using System;
using System.Collections.Generic;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.DataAccess.Interface.LogicInput;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic
{
    public class FileLogic : ElementLogic, ILogic<File>, IFileLogic
    {
        private IFileRepository FileRepository;

        private IElementValidator ElementValidator;

        public FileLogic(IFileRepository repository)
        {
            this.FileRepository = repository;
        }

        public FileLogic(IFileRepository repository, IElementValidator validator)
        {
            this.FileRepository = repository;
            this.ElementValidator = validator;
        }

        public void Create(File fileToCreate)
        {
            ElementValidator.IsValid(fileToCreate);
            FileRepository.Insert(fileToCreate);
            FileRepository.Save();
        }

        public void Delete(int id)
        {
            try
            {
                FileRepository.Delete(id);
                FileRepository.Save();
            }
            catch (Exception exception)
            {
                throw new LogicException(exception.Message, exception);
            }
        }

        public File Get(int Id)
        {
            return FileRepository.Get(Id);
        }

        public ICollection<File> GetAll(FileFilter filter)
        {
            return FileRepository.GetAll(filter);
        }

        public ICollection<File> GetAll()
        {
            return FileRepository.GetAll();
        }

        public void Update(File fileToUpdate)
        {
            ElementValidator.IsValid(fileToUpdate);
            fileToUpdate.DateModified = DateTime.Now;
            FileRepository.Update(fileToUpdate);
            FileRepository.Save();
        }
    }
}
