using MyRealTimeApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MyRealTimeApp.Infrastructure.Data
{
    //ef core bridge to SSMS
    //the Object-Relational Mapper (ORM) -> translates c# LINQ queries to raw SQL queries
    public class AppDbContext(DbContextOptions <AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tell EF Core that SqlTableDependency has attached a trigger to this table.
            // This stops EF Core from using the incompatible OUTPUT clause.
            modelBuilder.Entity<Product>()
                .ToTable(tb => tb.HasTrigger("SqlTableDependencyTrigger"));
        }
    }
}
