using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class FolderRepository : Repository<Folder>
    {
        public FolderRepository(TwoDriveDbContext current) : base(current)
        {
        }

        public override Folder Get(int Id)
        {
            return table
                .Include(f => f.FolderChilden)
                .Where(f => f.Id == Id)
                .FirstOrDefault();
        }

        public override ICollection<Folder> GetAll()
        {
            return table
                .Include(f => f.FolderChilden)
                .ToList();
        }

        public override bool Exists(Folder folder)
        {
            return table.Where(e => e.Name == folder.Name)
                        .Where(e => e.ParentFolderId == folder.ParentFolderId)
                        .Any();
        }
    }
}