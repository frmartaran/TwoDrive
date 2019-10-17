
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class ModificationRepository : Repository<Modification>
    {
        public ModificationRepository(TwoDriveDbContext current) : base(current)
        {
            table = context.Modifications;
        }
        public override ICollection<Modification> GetAll()
        {
            return table
                .IgnoreQueryFilters()
                .Include(m => m.ElementModified.Owner)
                .ToList();
        }
    }
}