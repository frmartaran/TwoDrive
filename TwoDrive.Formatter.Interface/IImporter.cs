using TwoDrive.BusinessLogic.Interfaces;

namespace TwoDrive.Importer.Interface
{
    public interface IImporter<ImportType>
        where ImportType : class
    {
        ILogic<ImportType> LogicToSave { get; set; }

        string FileExtension { get; set; }

        T Load<T>(string path) where T : class;

        void Import(string path);
    }
}
