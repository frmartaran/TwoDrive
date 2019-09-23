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
            Validator.isValid(writer);
            Repository.Insert(writer);
            Repository.Save();
        }

        public void Delete(Writer writer)
        {
            Repository.Delete(writer.Id);
            Repository.Save();
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
            Validator.isValid(writer);
            Repository.Update(writer);
        }
    }
}