using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Interfaces
{
    public interface IElementValidator : IValidator<Element>
    {
        bool ValidateDependenciesAreSet(IFolderRepository folderRepository, IFileRepository fileRepository);

        bool IsValidDestination(Element elementToTransfer, Element elementDestination);
    }
}