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
        private const string Spaces = "      ";
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
            ElementValidator.IsValid(folder);
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
                if (folder.FolderChildren.Count == 0)
                {
                    FolderRepository.Delete(element.Id);
                    FolderRepository.Save();
                    return;
                }
                var child = folder.FolderChildren.FirstOrDefault();
                while (child != null)
                {
                    DeleteChildren(child);
                    child = folder.FolderChildren.FirstOrDefault();
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
            ElementValidator.IsValid(folder);
            folder.DateModified = newDateModified;
            FolderRepository.Update(folder);
            FolderRepository.Save();
        }

        public object ShowTree(Folder root)
        {
            var tree = string.Format("{0} +- {1} \n", "", root.Name);
            ShowChildren(root, ref tree, Spaces);
            Console.Write(tree);
            return tree;
        }

        public void ShowChildren(Element element, ref string tree, string prefix)
        {
            if (element is Folder folder)
            {
                var folderWithChildren = FolderRepository.Get(folder.Id);
                var children = folderWithChildren.FolderChildren.ToList();
                if (children.Count == 0)
                    return;
                foreach (var child in children)
                {
                    if (children.IndexOf(child) == folder.FolderChildren.Count - 1)
                        tree += string.Format("{0} +- {1} \n", $"{prefix}\\", child.Name);
                    else
                        tree += string.Format("{0} +- {1} \n", $"{prefix}|", child.Name);
                    ShowChildren(child, ref tree, prefix + Spaces);
                }
                return;
            }
            else
            {
                return;
            }
        }
    }
}