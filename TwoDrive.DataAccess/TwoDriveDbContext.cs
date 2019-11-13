using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TwoDrive.Domain;
using TwoDrive.Domain.FileManagement;
using TwoDrive.Domain.Interface;

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
        public virtual DbSet<Modification> Modifications { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Element>()
                .Property<bool>("IsDeleted");

            modelBuilder.Entity<Element>()
                .HasQueryFilter(e => !e.IsDeleted);

            modelBuilder.Entity<WriterFriend>()
                .HasOne(wf => wf.Writer)
                .WithMany(w => w.Friends)
                .HasForeignKey(wf => wf.WriterId);

            modelBuilder.Entity<WriterFriend>()
                .HasOne(wf => wf.Friend)
                .WithMany()
                .HasForeignKey(wf => wf.FriendId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges()
        {
            SoftDelete();
            return base.SaveChanges();
        }

        private void SoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entry.Entity.GetType()))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.CurrentValues["IsDeleted"] = false;
                            break;
                        case EntityState.Deleted:
                            entry.CurrentValues["IsDeleted"] = true;
                            entry.CurrentValues["DeletedDate"] = DateTime.Now;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }
            }
        }
    }
}
