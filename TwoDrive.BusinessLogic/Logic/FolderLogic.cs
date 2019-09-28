using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.LogicInput;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Logic
{
    public class FolderLogic : ILogic<Folder>
    {
        private IRepository<Folder> FolderRepository { get; set; }

        private IValidator<Element> ElementValidator { get; set; }

        private IRepository<File> FileRepository { get; set; }

        private const string type = "Folder";
        public FolderLogic(IRepository<Folder> currentFolderRepository, IRepository<File> currentFileRepository)
        {
            FolderRepository = currentFolderRepository;
            FileRepository = currentFileRepository;
        }

        public FolderLogic(FolderLogicDependencies dependencies)
        {
            FolderRepository = dependencies.FolderRepository;
            ElementValidator = dependencies.ElementValidator;
            FileRepository = dependencies.FileRepository;
}
        public void Create(Folder folder)
        {
            ElementValidator.isValid(folder);
            FolderRepository.Insert(folder);
            FolderRepository.Save();
        }

        public void Delete(int id)
        {
            var folder = Get(id);
            DeleteChildren(folder);
        }

        private void DeleteChildren(Element element)
        {
            if (element is Folder folder)
            {
                if (folder.FolderChilden.Count == 0)
                {
                    FolderRepository.Delete(element.Id);
                    FolderRepository.Save();
                    return;
                }
                var child = folder.FolderChilden.FirstOrDefault();
                while (child != null)
                {
                    DeleteChildren(child);
                    child = folder.FolderChilden.FirstOrDefault();
                }
                DeleteChildren(element);
            }
            else
            {
                FileRepository.Delete(element.Id);
                FileRepository.Save();
                return;
            }
        }


        private static bool IsFile(Element folder)
        {
            return folder.GetType().IsSubclassOf(typeof(File));
        }

        public Folder Get(int Id)
        {
            return FolderRepository.Get(Id);
        }

        public ICollection<Folder> GetAll()
        {
            var allElements = FolderRepository.GetAll();
            return allElements;
        }

        public void Update(Folder folder)
        {
            var newDateModified = DateTime.Now;
            ElementValidator.isValid(folder);
            folder.DateModified = newDateModified;
            FolderRepository.Update(folder);
            FolderRepository.Save();
        }
    }
}