using System.Collections.Generic;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess.Interface
{
    public interface IFolderRepository : IRepository<Folder>
    {
        Folder GetRoot(int ownerId);

        ICollection<Element> GetChildren(int parentId);
    }
}