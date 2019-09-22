
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
            RecursiveDeleteChildren(folder);
            
        }

        private void RecursiveDeleteChildren(Element folder)
        {
            if (folder.GetType().IsSubclassOf(typeof(File)))
            {
                Repository.Delete(folder.Id);
                Repository.Save();
                return;
            }
            var asfolder = (Folder)folder;
            if (asfolder.FolderChilden.Count == 0)
            {
                Repository.Delete(folder.Id);
                Repository.Save();
                return;
            }
            var child = asfolder.FolderChilden.FirstOrDefault();
            while (child != null)
            {
                RecursiveDeleteChildren(child);
                child = asfolder.FolderChilden.FirstOrDefault();
            }
            RecursiveDeleteChildren(folder);

        }

        public Folder Get(int Id)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Folder> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void Update(Folder folder)
        {
            throw new System.NotImplementedException();
        }
    }
}