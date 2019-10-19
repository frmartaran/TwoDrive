using System;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.Domain;

namespace TwoDrive.Formatter.Interface
{
    public interface IFormatter<ImportType>
        where ImportType : class
    {
        ILogic<ImportType> LogicToSave { get; set; }

        string FileExtension { get; set; }

        Writer WriterFor { get; set; }

        T Load<T>(string path) where T : class;

        void Import(string path);
    }
}
