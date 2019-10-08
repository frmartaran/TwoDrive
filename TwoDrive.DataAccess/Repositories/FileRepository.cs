using System.Collections.Generic;
using System.Linq;
using TwoDrive.DataAccess.Extensions;
using TwoDrive.DataAccess.Interface;
using TwoDrive.DataAccess.Interface.LogicInput;
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

        public ICollection<File> GetAll(FileFilter filter)
        {
            var filesFilteredByName = filter.Id.HasValue
                ? table.Where(e => e.Name.Contains(filter.Name) && e.Owner.Id == filter.Id.Value)
                : table.Where(e => e.Name.Contains(filter.Name));

            if (filter.IsOrderDescending)
            {
                return FileRepositoryExtension.OrderByDescending(filter, filesFilteredByName);
            }
            else
            {
                return FileRepositoryExtension.OrderBy(filter, filesFilteredByName);
            }
        }
    }
}