using System;
using Microsoft.EntityFrameworkCore;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class TwoDriveDbContext : DbContext
    {
        public TwoDriveDbContext(DbContextOptions<TwoDriveDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Writer> Writers { get; set; }
        public virtual DbSet<Element> Elements { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<TxtFile> Txts { get; set; }

    }
}
