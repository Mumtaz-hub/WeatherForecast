using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContextFactory()
        {
        }

        public DatabaseContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=config;");
            return new DatabaseContext(builder.Options);
        }
    }
}
