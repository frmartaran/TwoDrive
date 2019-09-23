using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Logic
{
    public class FolderLogic : ILogic<Folder>
    {
        private IRepository<Element> Repository { get; set; }

        private IValidator<Element> Validator { get; set; }

        private const string type = "Folder";
        public FolderLogic(IRepository<Element> current)
        {
            Repository = current;
        }

        public FolderLogic(IRepository<Element> current, IValidator<Element> validator)
        {
            Repository = current;
            Validator = validator;
        }
        public void Create(Folder folder)
        {
            Validator.isValid(folder);
            Repository.Insert(folder);
            Repository.Save();
        }

        public void Delete(Folder folder)
        {
            DeleteChildren(folder);
            
        }

        private void DeleteChildren(Element element)
        {
            if (IsFile(element))
            {
                Repository.Delete(element.Id);
                Repository.Save();
                return;
            }
            var asfolder = (Folder)element;
            if (asfolder.FolderChilden.Count == 0)
            {
                Repository.Delete(element.Id);
                Repository.Save();
                return;
            }
            var child = asfolder.FolderChilden.FirstOrDefault();
            while (child != null)
            {
                DeleteChildren(child);
                child = asfolder.FolderChilden.FirstOrDefault();
            }
            DeleteChildren(element);
        }

        private static bool IsFile(Element folder)
        {
            return folder.GetType().IsSubclassOf(typeof(File));
        }

        public Folder Get(int Id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Folder> GetAll()
        {
            var allElements = Repository.GetAll();
            return allElements
                    .Where(f => f is Folder)
                    .ToList()
                    .ConvertAll(f => (Folder) f);
        }

        public void Update(Folder folder)
        {
            var newDateModified = DateTime.Now;
            Validator.isValid(folder);
            folder.DateModified = newDateModified;
            Repository.Update(folder);
            Repository.Save();
        }
    }
}