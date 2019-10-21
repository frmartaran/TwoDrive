using System.Collections.Generic;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.Importer.Interface
{
    public interface IImporter
    {
        string FileExtension { get; set; }

        T Load<T>(string path) where T : class;

        List<IFolder> Import(string path);
    }
}
