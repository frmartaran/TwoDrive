using System;
using System.Collections.Generic;
using System.Linq;
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

        public ICollection<IGrouping<int,Modification>> GetAllFromDateRange(DateTime startDate, DateTime endDate)
        {
            return Repository.GetAll()
                .Where(m => m.Date >= startDate)
                .Where(m => m.Date < endDate)
                .GroupBy(m => m.ElementId)
                .ToList();
        }
    }
}