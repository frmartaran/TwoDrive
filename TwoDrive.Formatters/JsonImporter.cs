using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.Importer
{
    public class JsonImporter : IImporter<IFolder>
    {
        public string FileExtension
        {
            get
            {
                return "JSON";
            }
           
        }

        public IFolder Import(string path)
        {
            throw new NotImplementedException();
        }

        public T Load<T>(string path) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
