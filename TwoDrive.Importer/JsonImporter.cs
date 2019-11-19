using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;
using TwoDrive.Importer.Domain;
using TwoDrive.Importer.Interface.Exceptions;

namespace TwoDrive.Importer
{
    public class JsonImporter : IImporter
    {
        private const string Name = "JSON";

        public ParameterDictionary ExtraParameters { get; set; }

        public JsonImporter()
        {
            ExtraParameters = new ParameterDictionary();
            ExtraParameters.AddParameter<string>("Test","Test value");
        }

        public string ImporterName
        {
            get
            {
                return Name;
            }

        }

        public T Import<T>(ImportingParameters parameters) where T : class
        {
            var binder = new KnownTypesBinder
            {
                KnownTypes = new List<Type>
                {
                    typeof(Domain.File),
                    typeof(Folder)
                }
            };
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                MissingMemberHandling = MissingMemberHandling.Error,
                MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                DateParseHandling = DateParseHandling.DateTime,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                SerializationBinder = binder
            };
            try
            {
                var jsonString = Load<string>(parameters);
                var folder = JsonConvert.DeserializeObject<Folder>(jsonString, settings);
                return folder as T;
            }
            catch (JsonSerializationException exception)
            {
                throw new ImporterException(ImporterResource.Json_Type_Exception, exception);
            }
            catch (JsonReaderException exception)
            {
                throw new ImporterException(ImporterResource.Json_Format_Exception, exception);
            }
        }

        public T Load<T>(ImportingParameters parameters) where T : class
        {
            try
            {
                using (var reader = new StreamReader(parameters.Path))
                {
                    return reader.ReadToEnd() as T;
                }
            }
            catch (FileNotFoundException exception)
            {
                throw new ImporterException(ImporterResource.FileNotFound_Exception, exception);
            }
            catch (ArgumentException exception)
            {
                throw new ImporterException(ImporterResource.EmptyPath, exception);
            }

        }

    }
}
