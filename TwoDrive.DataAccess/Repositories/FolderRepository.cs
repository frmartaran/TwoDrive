using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class FolderRepository : Repository<Folder>, IFolderRepository
    {
        public FolderRepository(TwoDriveDbContext current) : base(current)
        {
            table = current.Folders;
        }

        public override Folder Get(int Id)
        {
            return table
                .Include(f => f.FolderChildren)
                .Where(f => f.Id == Id)
                .FirstOrDefault();
        }

        public override ICollection<Folder> GetAll()
        {
            return table
                .Include(f => f.FolderChildren)
                .ToList();
        }

        public override bool Exists(Folder folder)
        {
            return table.Where(e => e.Name == folder.Name)
                        .Where(e => e.ParentFolderId == folder.ParentFolderId)
                        .Any();
        }

        public Folder GetRoot(int ownerId)
        {
            return table.Where(e => e.ParentFolderId == null && e.OwnerId == ownerId)
                        .FirstOrDefault();
        }
    }
}