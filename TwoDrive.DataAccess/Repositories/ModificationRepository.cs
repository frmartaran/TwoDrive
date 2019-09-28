
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class ModificationRepository : Repository<Modification>
    {
        public ModificationRepository(TwoDriveDbContext current) : base(current)
        {
        }
    }
}