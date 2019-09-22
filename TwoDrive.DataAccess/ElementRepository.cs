using System.Linq;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class ElementRepository : Repository<Element>
    {
        public ElementRepository(TwoDriveDbContext current) : base(current)
        {
        }

        public override bool Exists(Element element)
        {
            var type = element.GetType().Name;
            return table.Where(e => e.Name == element.Name)
                        .Where(e => e.GetType().Name == type)
                        .Where(e => e.ParentFolderId == element.ParentFolderId)
                        .Any();
        }

        public override Element Get(int Id)
        {
            return table.Find(Id);
        }
    }
}