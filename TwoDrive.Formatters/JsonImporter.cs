using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;
using TwoDrive.Importer.Domain;
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
            var binder = new KnownTypesBinder
            {
                KnownTypes = new List<Type>
                {
                    typeof(Domain.File),
                    typeof(Folder)
                }
            };
            var jsonString = Load<string>(path);
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                MissingMemberHandling = MissingMemberHandling.Error,
                NullValueHandling = NullValueHandling.Include,
                MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                Binder = binder
            };
            try
            {
                var folder = JsonConvert.DeserializeObject<Folder>(jsonString, settings);
                return folder;
            }
            catch (JsonSerializationException exception)
            {
                throw new ImporterException(ImporterResource.MissingType_Exception, exception);
            }
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
