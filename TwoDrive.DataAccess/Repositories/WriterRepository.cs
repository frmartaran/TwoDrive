using TwoDrive.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace TwoDrive.DataAccess
{

    public class WriterRepository : Repository<Writer>
    {
        public WriterRepository(TwoDriveDbContext current) : base(current)
        {
            table = current.Writers;
        }
        public override bool Exists(Writer writer)
        {
            return table.Where(w => w.UserName == writer.UserName)
                        .Where(w => w.Id != writer.Id)
                        .Any();
        }

        public override Writer Get(int Id)
        {
            return table.Include(w => w.Claims)
                            .ThenInclude(c => c.Element)
                                .ThenInclude(e => e.Owner)
                        .Include(w => w.Friends)
                            .ThenInclude(wf => wf.Friend)
                        .Where(w => w.Id == Id)
                        .FirstOrDefault();
        }

        public override ICollection<Writer> GetAll()
        {
            return table
                .Include(w => w.Claims)
                    .ThenInclude(c => c.Element)
                .Include(w => w.Friends)
                    .ThenInclude(wf => wf.Friend)
                .ToList();
        }
    }
}