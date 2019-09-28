
using System.Linq;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Extensions
{
    public static class WriterLogicExtension
    {
        public static bool HasClaimsFor(this Writer writer, Element element, ClaimType type)
        {
            return writer.Claims
                    .Where(c => c.Element == element)
                    .Where(c => c.Type == type)
                    .Any();
        }

        public static bool IsFriendsWith(this Writer writer, Writer friend)
        {
            return writer.Friends.Contains(friend);
        }
    }
}