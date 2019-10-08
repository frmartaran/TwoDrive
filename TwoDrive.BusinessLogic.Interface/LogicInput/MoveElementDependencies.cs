using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Interfaces.LogicInput
{
    public class MoveElementDependencies
    {
        public IRepository<Element> ElementRepository { get; set; }

        public IFolderValidator ElementValidator { get; set; }
    }
}
