using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
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
            var beforeStart = startDate.CompareTo(endDate);
            if(beforeStart > 0)
                throw new LogicException("End date cannot be earlier than start date");
                
            return Repository.GetAll()
                .Where(m => m.Date >= startDate)
                .Where(m => m.Date < endDate)
                .GroupBy(m => m.ElementId)
                .ToList();
        }
    }
}