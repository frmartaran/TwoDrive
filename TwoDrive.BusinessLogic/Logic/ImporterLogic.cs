using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
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
    public class ImporterLogic
    {
        private const string DllToImport = "TwoDrive.Importer.dll";

        private const string fieldName = "Extension";
        private IFolderLogic FolderLogic { get; set; }

        private IFileLogic FileLogic { get; set; }

        private ILogic<Writer> WriterLogic { get; set; }

        private ImportingOptions Options { get; set; }

        private IModificationLogic ModificationLogic { get; set; }

        public ImporterLogic(ImportingOptions options, ImporterLogicDependencies dependencies)
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
            var importer = GetImporter();
            var parentFolder = importer.Import(Options.FilePath);
            var mapper = MapperHelper.GetFileManagementMapper();

            var domainFolder = mapper.Map<IFolder, Folder>(parentFolder);
            domainFolder.Owner = Options.Owner;

            var childrenList = domainFolder.FolderChildren;
            domainFolder.FolderChildren = new List<Element>();

            FolderLogic.Create(domainFolder);

            foreach (var child in childrenList)
            {
                child.ParentFolder = domainFolder;
                var owner = domainFolder.Owner;
                child.Owner = owner;
                if (child is Folder)
                {
                    FolderLogic.Create(child as Folder);
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

            Options.Owner.AddRootClaims(domainFolder);
            WriterLogic.Update(Options.Owner);
            CreateImportModification(domainFolder);
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
    }
}
