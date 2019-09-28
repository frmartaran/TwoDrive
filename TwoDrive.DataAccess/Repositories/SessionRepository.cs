
using System.Linq;
using TwoDrive.Domain;

namespace TwoDrive.DataAccess
{
    public class SessionRepository : Repository<Session>
    {
        public SessionRepository(TwoDriveDbContext current) : base(current)
        {
        }

        public override bool Exists(Session session){

            return context.Sessions
                    .Where(s => s.Token == session.Token)
                    .Any();
        }

        
    }
}