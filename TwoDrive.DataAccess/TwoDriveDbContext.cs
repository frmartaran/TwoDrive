﻿using System;
using Microsoft.EntityFrameworkCore;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess
{
    public class TwoDriveDbContext : DbContext
    {
        public TwoDriveDbContext(DbContextOptions<TwoDriveDbContext> options) : base(options)
        {
            
        }

        public DbSet<Element> Elements { get; set; }

        public DbSet<Folder> Folders { get; set; }
    }
}