using System.Collections.Generic;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.Importer.Interface
{
    public interface IImporter
    {
        string FileExtension { get;}

        T Load<T>(ImportingParameters paramenters) where T : class;

        ImportType Import<ImportType>(ImportingParameters parameters) 
            where ImportType : class;
    }
}
