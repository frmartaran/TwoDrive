using System.Linq;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class FileRepository : Repository<File>
    {
        public FileRepository(TwoDriveDbContext current) : base(current)
        {
        }

        public override bool Exists(File file)
        {
            return table.Where(e => e.Name == file.Name)
                        .Where(e => e.ParentFolderId == file.ParentFolderId)
                        .Any();
        }
    }
}