using System;
using Microsoft.EntityFrameworkCore;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class TwoDriveDbContext : DbContext
    {
        public TwoDriveDbContext(){}
        public TwoDriveDbContext(DbContextOptions<TwoDriveDbContext> options) : base(options)
        {
            
        }

        public virtual DbSet<Element> Elements { get; set; }

        public virtual DbSet<Folder> Folders { get; set; }
    }
}
