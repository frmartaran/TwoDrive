using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;
using TwoDrive.Importer.MockDomain;
using TwoDrive.Importers.Exceptions;

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
            var jsonString = Load<string>(path);
            var folder = JsonConvert.DeserializeObject<MockFolder>(jsonString);
            return folder;
        }

        public T Load<T>(string path) where T : class
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    return reader.ReadToEnd() as T;
                }
            }
            catch (FileNotFoundException exception)
            {
                throw new ImporterException(ImporterResource.FileNotFound_Exception, exception);
            }

        }
    }
}
