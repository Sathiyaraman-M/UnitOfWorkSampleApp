using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using UnitOfWorkSampleApp.Common;

namespace UnitOfWorkSampleApp;
public class LibraryDbContext : DbContext
{
    private readonly string _userId = "sbx5789t9my3981p9ym129ndt129m"; //Some random text for demo purposes
    private readonly string _userName = "Administrator";
    
    // Do not put connection strings inside the code for production applications. Only for demo purposes
    private readonly string _connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=SampleConsoleProject;Integrated Security=True;MultipleActiveResultSets=True";

    // Add DbSets for your database tables here
    public DbSet<Book> Books { get; set; }
    public DbSet<BookHeader> BookHeaders { get; set; }

    // Overriding the base function to add additional entity details before saving them to database
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedOn = DateTime.Now;
                    entry.Entity.CreatedByUserId = _userId;
                    entry.Entity.CreatedBy = _userName;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.Now;
                    entry.Entity.LastModifiedByUserId = _userId;
                    entry.Entity.LastModifiedBy = _userName;
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Using Microsoft SQL Server as backend.
        // You can use any database by installing appropriate nuget package for EF Core database provider.
        // For example, for Sqlite, add Microsoft.EntityFrameworkCore.Sqlite package.
        optionsBuilder.UseSqlServer(_connectionString);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Configuring space size for decimal columns like 'Cost' to avoid any invalid truncation
        foreach (var property in builder.Model.GetEntityTypes().SelectMany(t => t.GetProperties())
        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,2)");
        }
    }
}