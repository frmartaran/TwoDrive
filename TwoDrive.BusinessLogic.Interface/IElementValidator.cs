using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface IElementValidator : IValidator<Element>
    {
        bool IsValidDestination(Element elementToTransfer, Folder folderDestination);
    }
}