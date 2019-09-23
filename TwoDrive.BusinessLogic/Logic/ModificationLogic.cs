using System;
using System.Collections.Generic;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Logic
{
    public class ModificationLogic
    {
        private IRepository<Modification> Repository { get; set; }

        public ModificationLogic(IRepository<Modification> repository)
        {
            Repository = repository;
        }

        public void Create(Modification modification)
        {
            Repository.Insert(modification);
            Repository.Save();
        }

        public ICollection<Modification> GetAllFromDateRange(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}