using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface IElementLogic
    {
        void MoveElement(Element elementToMove, Element elementDestination);
    }
}