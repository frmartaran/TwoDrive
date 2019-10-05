using TwoDrive.Domain;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using TwoDrive.DataAccess.Exceptions;

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