using System.Collections.Generic;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.Importer.Interface
{
    public interface IImporter<ImportType>
        where ImportType : class
    {
        string FileExtension { get; set; }

        T Load<T>(string path) where T : class;

        List<ImportType> Import(string path);
    }
}
