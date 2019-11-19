using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Helpers;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.BusinessLogic.Resources;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.BusinessLogic.Logic
{
    public class ImporterLogic : IImporterLogic
    {
        private const string fieldName = "Name";

        private IFolderLogic FolderLogic { get; set; }

        private IFileLogic FileLogic { get; set; }

        private ILogic<Writer> WriterLogic { get; set; }

        public ImportingOptions Options { get; set; }

        private IModificationLogic ModificationLogic { get; set; }

        public ImporterLogic(ImporterDependencies dependencies)
        {
            FolderLogic = dependencies.FolderLogic;
            FileLogic = dependencies.FileLogic;
            WriterLogic = dependencies.WriterLogic;
            ModificationLogic = dependencies.ModificationLogic;
        }

        public IImporter GetImporter(string path)
        {
            try
            {
                var assemblyInfo = Assembly.LoadFrom(path);
                var applicablesTypes = assemblyInfo.ExportedTypes
                    .Where(t => (typeof(IImporter).IsAssignableFrom(t)))
                    .ToList();

                var importerType = applicablesTypes
                    .Where(t => t.GetField(fieldName, BindingFlags.NonPublic
                                                      | BindingFlags.Static)
                        .GetRawConstantValue() as string == Options.ImporterName)
                    .SingleOrDefault();

                var instance = Activator.CreateInstance(importerType);
                return instance as IImporter;
            }
            catch (ArgumentNullException exception)
            {
                throw new ImporterNotFoundException(BusinessResource.ImporterNotFound_ImporterLogic, exception);
            }
            catch (TypeLoadException exception)
            {
                throw new LogicException(BusinessResource.NeedsRedeployment, exception);
            }

        }

        public void Import(string path)
        {
            if (Options.Owner == null)
                throw new LogicException(BusinessResource.MissingOwner);

            var importer = GetImporter(path);
            var parentFolder = importer.Import<IFolder>(Options.Parameters);
            var mapper = MapperHelper.GetFileManagementMapper();

            Folder domainFolder;
            try
            {
                domainFolder = mapper.Map<IFolder, Folder>(parentFolder);

                domainFolder.Owner = Options.Owner;
                var childrenList = domainFolder.FolderChildren;
                domainFolder.FolderChildren = new List<Element>();

                FolderLogic.Create(domainFolder);
                ImportChildren(domainFolder, childrenList);
            }
            catch (AutoMapperMappingException exception)
            {
                throw new LogicException(BusinessResource.MappingError_Mapper, exception);
            }
            catch (ValidationException exception)
            {
                if (!(exception is DuplicateResourceException))
                {
                    var root = FolderLogic.GetRootFolder(Options.Owner);
                    FolderLogic.Delete(root.Id);
                }

                throw new LogicException(exception.Message, exception);

            }

            Options.Owner.AddRootClaims(domainFolder);
            WriterLogic.Update(Options.Owner);
            CreateImportModification(domainFolder);
        }

        private void ImportChildren(Folder parentFolder, ICollection<Element> children)
        {
            foreach (var child in children)
            {
                child.ParentFolder = parentFolder;
                var owner = parentFolder.Owner;
                child.Owner = owner;
                if (child is Folder folder)
                {
                    var innerChildren = folder.FolderChildren;
                    folder.FolderChildren = new List<Element>();
                    FolderLogic.Create(folder);
                    ImportChildren(folder, innerChildren);
                }
                else
                {
                    FileLogic.Create(child as Domain.FileManagement.File);
                }

                owner.AddCreatorClaimsTo(child);
                WriterLogic.Update(owner);
                CreateImportModification(child);
                FolderLogic.CreateModificationsForTree(child, ModificationType.Changed);
            }
        }

        private void CreateImportModification(Element domainFolder)
        {
            var modification = new Modification
            {
                Date = DateTime.Now,
                ElementModified = domainFolder,
                type = ModificationType.Imported
            };
            ModificationLogic.Create(modification);
        }

        public List<ImporterInfo> GetAllImporters(string path)
        {
            try
            {
                var assemblyInfo = Assembly.LoadFrom(path);
                var allImporters = assemblyInfo.GetTypes()
                    .Where(t => (typeof(IImporter).IsAssignableFrom(t)))
                    .ToList();
                var importersInfo = new List<ImporterInfo>();
                foreach (var importer in allImporters)
                {
                    var importerInstance = Activator.CreateInstance(importer) as IImporter;

                    var name = importer.GetProperty("ImporterName").GetValue(importerInstance)
                        as string;

                    var info = new ImporterInfo
                    {
                        Name = name,
                        Parameters = importerInstance.ExtraParameters
                    };
                    importersInfo.Add(info);

                }
                return importersInfo;
            }
            catch (TypeLoadException exception)
            {
                throw new LogicException(BusinessResource.NeedsRedeployment, exception);
            }
            catch (ArgumentException exception)
            {
                throw new LogicException(BusinessResource.DllNotFound, exception);
            }
            catch (FileNotFoundException exception)
            {
                throw new LogicException(BusinessResource.DllNotFound, exception);
            }

        }
    }
}
