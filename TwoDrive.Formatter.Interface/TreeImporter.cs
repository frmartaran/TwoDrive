using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.Importer.Interface
{
    public interface ITreeImporter : IImporter<Folder>
    {
        IFileLogic FileLogic { get; set; }

        Writer WriterFor { get; set; }

    }
}
