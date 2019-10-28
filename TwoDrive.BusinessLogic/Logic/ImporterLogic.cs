using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TwoDrive.BusinessLogic.Exceptions;
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

        public ImporterLogic(ImportingOptions options, ImporterLogicDependencies dependencies)
        {
            Options = options;
            FolderLogic = dependencies.FolderLogic;
            FileLogic = dependencies.FileLogic;
            WriterLogic = dependencies.WriterLogic;
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
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<IElement, Element>()
                .Include<IFolder, Folder>()
                .Include<IFile, File>()
                .ForMember(src => src.IsDeleted, opt => opt.Ignore())
                .ForMember(src => src.DeletedDate, opt => opt.Ignore())
                .ForMember(src => src.Id, opt => opt.Ignore())
                .ForMember(src => src.Owner, opt => opt.Ignore())
                .ForMember(src => src.OwnerId, opt => opt.Ignore())
                .ForMember(src => src.ParentFolderId, opt => opt.Ignore());

                cfg.CreateMap<IFile, File>()
                    .Include<IFile, TxtFile>()
                    .Include<IFile, HTMLFile>();

                cfg.CreateMap<IFolder, Folder>()
                .ForMember(src => src.FolderChildren, opt => opt.MapFrom(f => f.FolderChildren));

                cfg.CreateMap<IFile, HTMLFile>()
                .ForMember(src => src.ShouldRender, opt => opt.MapFrom(f => f.ShouldRender));

                cfg.CreateMap<IFile, TxtFile>();

            });
            var mapper = config.CreateMapper();
            var domainFolder = mapper.Map<IFolder, Folder>(parentFolder);
            FolderLogic.Create(domainFolder);

        }
    }
}
