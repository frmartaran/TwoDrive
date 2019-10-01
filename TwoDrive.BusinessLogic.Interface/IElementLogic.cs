using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Interface
{
    public interface IElementLogic
    {
        void MoveElement(Element elementToMove, Folder elementDestination);
    }
}