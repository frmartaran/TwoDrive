using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;
using TwoDrive.Importer.Domain;
using TwoDrive.Importer.Interface.Exceptions;
using TwoDrive.Importer.Parameters;

namespace TwoDrive.Importer
{
    public class JsonImporter : IImporter
    {
        private const string Name = "JSON";

        public string ImporterName
        {
            get
            {
                return Name;
            }

        }

        public Type ParameterType
        {
            get
            {
                return typeof(JsonParameter);
            }
        }

        public ParameterDictionary GetExtraParameters()
        {
            throw new NotImplementedException();
        }

        public T Import<T>(ImportingParameters parameters) where T : class
        {
            var param = parameters as JsonParameter;
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
                var jsonString = Load<string>(param);
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
                var param = parameters as JsonParameter;
                using (var reader = new StreamReader(param.Path))
                {
                    return reader.ReadToEnd() as T;
                }
            }
            catch (FileNotFoundException exception)
            {
                throw new ImporterException(ImporterResource.FileNotFound_Exception, exception);
            }

        }

        public void SetExtraParameters()
        {
            throw new NotImplementedException();
        }
    }
}
