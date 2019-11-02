using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Importer.Interface.IFileManagement;

namespace TwoDrive.BusinessLogic.Helpers
{
    public class MapperHelper
    {
        public static IMapper GetFileManagementMapper()
        {
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
                    .Include<IFile, HTMLFile>()
                    .ConstructUsing((src, opt) => {
                        switch (src.Type)
                        {
                            case "HTML":
                                return opt.Mapper.Map<HTMLFile>(src);
                            case "TXT":
                                return opt.Mapper.Map<TxtFile>(src);
                            default:
                                throw new LogicException("");
                        }

                    });

                cfg.CreateMap<IFolder, Folder>()
                .ForMember(src => src.FolderChildren, opt => opt.MapFrom(f => f.FolderChildren));

                cfg.CreateMap<IFile, HTMLFile>()
                .ForMember(src => src.ShouldRender, opt => opt.MapFrom(f => f.ShouldRender));

                cfg.CreateMap<IFile, TxtFile>();

            });

            return config.CreateMapper();
        }
    }
}
