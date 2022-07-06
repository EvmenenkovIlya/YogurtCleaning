using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;

namespace YogurtCleaning.DataLayer;

public class YogurtCleaningContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Cleaner> Cleaners { get; set; }
    public DbSet<CleaningObject> CleaningObjects { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Bundle> Bundles { get; set; }
    public DbSet<Service> Services { get; set; }

    public YogurtCleaningContext(DbContextOptions<YogurtCleaningContext> options)
            : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RemovePluralizingTableNameConvention();

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable(nameof(Order));
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.CleaningObject);
            
            //entity.HasMany<Cleaner>
        });
        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable(nameof(Client));
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Addresses).WithOne(co => co.Client);
        });

        modelBuilder.Entity<Cleaner>(entity =>
        {
            entity.ToTable(nameof(Cleaner));
            entity.HasKey(e => e.Id);
            //entity.HasMany(e => e.Districts).WithOne(co => co.Client);
        });

        modelBuilder.Entity<CleaningObject>(entity =>
        {
            entity.ToTable(nameof(CleaningObject));
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Client);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable(nameof(Comment));
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Client);
            entity.HasOne(e => e.Cleaner);
            entity.HasOne(e => e.Order);
        });

        modelBuilder.Entity<Bundle>(entity =>
        {
            entity.ToTable(nameof(Bundle));
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable(nameof(Service));
        });
    }
}