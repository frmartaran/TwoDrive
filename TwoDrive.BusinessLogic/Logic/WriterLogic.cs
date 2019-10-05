using System;
using System.Collections.Generic;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;

namespace TwoDrive.BusinessLogic.Logic
{
    public class WriterLogic : ILogic<Writer>
    {
        private IRepository<Writer> Repository { get; set; }
        private IValidator<Writer> Validator { get; set; }

        public WriterLogic(IRepository<Writer> currentRepository)
        {
            Repository = currentRepository;
        }
        public WriterLogic(IRepository<Writer> currentRepository, IValidator<Writer> CurrentValidator)
        {
            Repository = currentRepository;
            Validator = CurrentValidator;
        }
        public void Create(Writer writer)
        {
            Validator.IsValid(writer);
            Repository.Insert(writer);
            Repository.Save();
        }

        public void Delete(int id)
        {
            try
            {
                Repository.Delete(id);
                Repository.Save();
            }
            catch (Exception exception)
            {
                throw new LogicException(exception.Message, exception);
            }
        }

        public Writer Get(int Id)
        {
            return Repository.Get(Id);
        }

        public ICollection<Writer> GetAll()
        {
            return Repository.GetAll();
        }

        public void Update(Writer writer)
        {
            Validator.IsValid(writer);
            Repository.Update(writer);
        }
    }
}