using TwoDrive.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TwoDrive.DataAccess
{

    public class WriterRepository : Repository<Writer>
    {
        public WriterRepository(TwoDriveDbContext current) : base(current)
        {
            table = current.Set<Writer>();
        }
        public override bool Exists(Writer writer)
        {
            var holi = table.ToList();
            return table.Where(w => w.UserName.Equals(writer.UserName))
                        .Any();
        }

        public override Writer Get(int Id)
        {
            return table.Include(w => w.Claims)
                        .Include(w => w.Friends)
                        .Where(w => w.Id == Id)
                        .FirstOrDefault();
        }
    }
}