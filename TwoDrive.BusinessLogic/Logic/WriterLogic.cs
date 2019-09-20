using System;
using System.Collections.Generic;
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
            var isValid = Validator.isValid(writer);
            Repository.Insert(writer);
            Repository.Save();
        }

        public void Delete(Writer objectToCreate)
        {
            throw new NotImplementedException();
        }

        public Writer Get(int Id)
        {
            return Repository.Get(Id);
        }

        public ICollection<Writer> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Writer writer)
        {
            Repository.Update(writer);
        }
    }
}