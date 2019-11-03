using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Extensions;
using TwoDrive.BusinessLogic.Helpers;
using TwoDrive.BusinessLogic.Helpers.LogicInput;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Resources;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Importer.Interface;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.BusinessLogic.Logic
{
    public class ImporterLogic : IImporterLogic
    {
        private const string DllToImport = "TwoDrive.Importer.dll";

        private const string fieldName = "Extension";
        private IFolderLogic FolderLogic { get; set; }

        private IFileLogic FileLogic { get; set; }

        private ILogic<Writer> WriterLogic { get; set; }

        private ImportingOptions Options { get; set; }

        private IModificationLogic ModificationLogic { get; set; }

        public ImporterLogic(ImportingOptions options, ImporterDependencies dependencies)
        {
            Options = options;
            FolderLogic = dependencies.FolderLogic;
            FileLogic = dependencies.FileLogic;
            WriterLogic = dependencies.WriterLogic;
            ModificationLogic = dependencies.ModificationLogic;
        }

        public IImporter<IFolder> GetImporter()
        {
            try
            {
                var assemblyInfo = Assembly.LoadFrom(DllToImport);
                var applicablesTypes = assemblyInfo.ExportedTypes
                    .Where(t => (typeof(IImporter<IFolder>).IsAssignableFrom(t)))
                    .ToList();

                var importerType = applicablesTypes
                    .Where(t => t.GetField(fieldName, BindingFlags.NonPublic
                                                      | BindingFlags.Static)
                        .GetRawConstantValue() as string == Options.FileType)
                    .SingleOrDefault();

                var instance = Activator.CreateInstance(importerType);
                return instance as IImporter<IFolder>;
            }
            catch (ArgumentNullException exception)
            {
                throw new ImporterNotFoundException(BusinessResource.ImporterNotFound_ImporterLogic, exception);
            }

        }

        public void Import()
        {
            if (Options.Owner == null)
                throw new LogicException(BusinessResource.MissingOwner);

            var importer = GetImporter();
            var parentFolder = importer.Import(Options.FilePath);
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
                    FileLogic.Create(child as File);
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

        public List<string> GetAllImporters()
        {
            throw new NotImplementedException();
        }
    }
}
