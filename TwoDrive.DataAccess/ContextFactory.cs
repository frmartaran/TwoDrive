using System;
using Microsoft.EntityFrameworkCore;

namespace TwoDrive.DataAccess
{

    public class ContextFactory
    {
        public static TwoDriveDbContext GetMemoryContext(string nameDb)
        {
            var builder = new DbContextOptionsBuilder<TwoDriveDbContext>();
            return new TwoDriveDbContext(GetMemoryConfig(nameDb, builder));
        }

        private static DbContextOptions<TwoDriveDbContext> GetMemoryConfig(string nameDb, DbContextOptionsBuilder<TwoDriveDbContext> builder)
        {
            builder.UseInMemoryDatabase(nameDb);
            return builder.Options;
        }
    }
}