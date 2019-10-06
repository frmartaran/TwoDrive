using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface IModificationLogic
    {
        void Create(Modification modification);

        ICollection<IGrouping<Element, Modification>> GetAllFromDateRange(DateTime startDate, DateTime endDate);
    }
}
