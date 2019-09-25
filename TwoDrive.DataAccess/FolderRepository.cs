using System.Linq;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class FolderRepository : Repository<Folder>
    {
        public FolderRepository(TwoDriveDbContext current) : base(current)
        {
        }

        public override bool Exists(Folder folder)
        {
            return table.Where(e => e.Name == folder.Name)
                        .Where(e => e.ParentFolderId == folder.ParentFolderId)
                        .Any();
        }
    }
}