using System;
using System.Collections.Generic;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.Importer.Interface
{
    public interface IImporter
    {
        string ImporterName { get; }
        ParameterDictionary ExtraParameters { get; set; }
        T Load<T>(ImportingParameters paramenters) where T : class;

        ImportType Import<ImportType>(ImportingParameters parameters)
            where ImportType : class;
    }
}
