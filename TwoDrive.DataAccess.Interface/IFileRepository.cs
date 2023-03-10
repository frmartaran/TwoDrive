using System.Collections.Generic;
using TwoDrive.DataAccess.Interface.LogicInput;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess.Interface
{
    public interface IFileRepository : IRepository<File>
    {
        ICollection<File> GetAll(FileFilter filter);
    }
}