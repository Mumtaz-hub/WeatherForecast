using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Weather> Weather { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
