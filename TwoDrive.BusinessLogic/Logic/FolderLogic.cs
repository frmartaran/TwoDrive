using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.BusinessLogic.Exceptions;
using TwoDrive.BusinessLogic.Interfaces;
using TwoDrive.BusinessLogic.Interfaces.LogicInput;
using TwoDrive.BusinessLogic.Resources;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.BusinessLogic.Logic
{
    public class FolderLogic : ElementLogic, ILogic<Folder>, IFolderLogic
    {
        private IFolderRepository FolderRepository { get; set; }

        private IFolderValidator ElementValidator { get; set; }

        private IFileRepository FileRepository { get; set; }

        private IRepository<Modification> ModificationRepository { get; set; }

        private const string Spaces = "      ";

        public FolderLogic(ElementLogicDependencies dependencies)
        {
            FolderRepository = dependencies.FolderRepository;
            ElementValidator = dependencies.ElementValidator;
            FileRepository = dependencies.FileRepository;
            ModificationRepository = dependencies.ModificationRepository;
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
            if (folder == null)
                throw new LogicException(BusinessResource.FolderNotFound);

            DeleteChildren(folder);
        }

        private void DeleteChildren(Element element)
        {
            if (element is Folder folder)
            {
                var folderAndChildren = FolderRepository.Get(folder.Id);
                var children = folderAndChildren
                    .FolderChildren
                    .Where(c => !c.IsDeleted)
                    .ToList();
                if (children.Count == 0)
                {
                    AddDeleteModfication(element);
                    FolderRepository.Delete(element.Id);
                    FolderRepository.Save();
                    return;
                }
                var child = folder.FolderChildren
                    .Where(c => !c.IsDeleted)
                    .FirstOrDefault();
                while (child != null)
                {
                    DeleteChildren(child);
                    child = folder.FolderChildren
                        .Where(c => !c.IsDeleted)
                        .FirstOrDefault();
                }
                DeleteChildren(element);
            }
            else
            {
                var file = FileRepository.Get(element.Id);
                AddDeleteModfication(file);
                FileRepository.Delete(file.Id);
                FileRepository.Save();
                return;
            }
        }

        private void AddDeleteModfication(Element element)
        {
            var modification = new Modification
            {
                ElementModified = element,
                type = ModificationType.Deleted,
                Date = DateTime.Now
            };

            CreateModificationsForTree(element, ModificationType.Deleted);
            ModificationRepository.Insert(modification);
            ModificationRepository.Save();
        }

        public void CreateModificationsForTree(Element element, ModificationType action)
        {
            var now = DateTime.Now;
            if (element.ParentFolder != null)
            {
                var parent = FolderRepository.Get(element.ParentFolder.Id);
                UpdateFolderTree(parent, now, action);
            }

        }

        private void UpdateFolderTree(Folder folder, DateTime now, ModificationType action)
        {
            CreateModificationForParentFolder(folder, now, action);
            if (folder.ParentFolder == null)
                return;

            var parentFromDb = FolderRepository.Get(folder.ParentFolder.Id);
            UpdateFolderTree(parentFromDb, now, action);

        }

        private void CreateModificationForParentFolder(Folder parent, DateTime now, ModificationType action)
        {
            var modification = new Modification
            {
                ElementModified = parent,
                type = action,
                Date = now
            };
            parent.DateModified = now;
            FolderRepository.Update(parent);
            ModificationRepository.Insert(modification);
            ModificationRepository.Save();
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

        public string ShowTree(Folder root)
        {
            var tree = string.Format(BusinessResource.ShowTreeFormat + " \n", "", root.Name);
            ShowChildren(root, ref tree, Spaces);
            return tree;
        }

        private void ShowChildren(Element element, ref string tree, string prefix)
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
                        tree += string.Format(BusinessResource.ShowTreeFormat + " \n", $"{prefix}\\", child.Name);
                    else
                        tree += string.Format(BusinessResource.ShowTreeFormat + " \n", $"{prefix}|", child.Name);
                    ShowChildren(child, ref tree, prefix + Spaces);
                }
                return;
            }
            else
            {
                return;
            }
        }

        public Folder GetRootFolder(Writer owner)
        {
            return FolderRepository.GetRoot(owner.Id);
        }
    }
}