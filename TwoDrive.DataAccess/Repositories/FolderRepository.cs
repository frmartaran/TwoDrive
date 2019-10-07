using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TwoDrive.DataAccess.Exceptions;
using TwoDrive.DataAccess.Interface;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class FolderRepository : Repository<Folder>, IFolderRepository
    {
        public FolderRepository(TwoDriveDbContext current) : base(current)
        {
            table = current.Folders;
        }

        public override Folder Get(int Id)
        {
            return table
                .Include(f => f.FolderChildren)
                .Where(f => f.Id == Id)
                .FirstOrDefault();
        }

        public override ICollection<Folder> GetAll()
        {
            return table
                .Include(f => f.FolderChildren)
                .ToList();
        }

        public override bool Exists(Folder folder)
        {
            return table.Where(e => e.Name == folder.Name)
                        .Where(e => e.ParentFolderId == folder.ParentFolderId)
                        .Any();
        }

        public Folder GetRoot(int ownerId)
        {
            return table.Where(e => e.ParentFolderId == null && e.OwnerId == ownerId)
                        .Include(r => r.FolderChildren)
                        .FirstOrDefault();
        }

        public ICollection<Element> GetChildren(int parentId)
        {
            try
            {
                return table.Where(e => e.Id == parentId)
                    .Include(c => c.FolderChildren)
                    .FirstOrDefault()
                    .FolderChildren;
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException($"There is no parent with id {parentId} in the database.");
            }
        }
    }
}