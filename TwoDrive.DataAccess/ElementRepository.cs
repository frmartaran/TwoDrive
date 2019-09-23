using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public ICollection<Folder> GetAllFolders()
        {

            return context.Folders
                .Include(f => f.Owner)
                .Include(f => f.ParentFolder)
                .Include(f => f.FolderChilden)
                .ToList();

        }

        public ICollection<File> GetAllFiles()
        {
            return context.Files
                .Include(f => f.Owner)
                .Include(f => f.ParentFolder)
                .ToList();
        }

    }
}