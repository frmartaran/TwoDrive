using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.DataAccess.Interface.LogicInput;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface IFileLogic : ILogic<File>, IElementLogic
    {
        ICollection<File> GetAll(FileFilter filter);
    }
}
