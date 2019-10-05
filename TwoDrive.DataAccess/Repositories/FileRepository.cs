using System.Linq;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class FileRepository : Repository<File>, IFileRepository
    {
        public FileRepository(TwoDriveDbContext current) : base(current)
        {
            table = current.Files;
        }

        public override bool Exists(File file)
        {
            return table.Where(e => e.Name == file.Name)
                        .Where(e => e.ParentFolderId == file.ParentFolderId)
                        .Any();
        }
    }
}