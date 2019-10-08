
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.Domain;

namespace TwoDrive.DataAccess
{
    public class SessionRepository : Repository<Session>
    {
        public SessionRepository(TwoDriveDbContext current) : base(current)
        {
        }

        public override ICollection<Session> GetAll()
        {
            return context.Sessions
                .Include(s => s.Writer)
                .ToList();
        }

        public override bool Exists(Session session){

            return context.Sessions
                    .Where(s => s.Token == session.Token)
                    .Any();
        }

        
    }
}