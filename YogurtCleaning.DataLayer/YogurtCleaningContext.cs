using Microsoft.EntityFrameworkCore;
using YogurtCleaning.DataLayer.Entities;
using YogurtCleaning.DataLayer.Enums;
using YogurtCleaning.DataLayer.Seed;

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
    public DbSet<District> Districts { get; set; }
    public YogurtCleaningContext(DbContextOptions<YogurtCleaningContext> options)
            : base(options)
    {
        //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<District>(entity =>
        {
            entity.ToTable(nameof(District));
            entity.HasData(EnumFunctions.GetModelsFromEnum<District, DistrictEnum>());
            entity.HasMany(d => d.Cleaners).WithMany(c => c.Districts);

        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable(nameof(Order));
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.CleaningObject).WithMany(c => c.Orders);
            entity.HasMany(e => e.Services).WithMany(s => s.Orders);
            entity.HasMany(e => e.Bundles).WithMany(b => b.Orders);
            entity.HasMany<Cleaner>(e => e.CleanersBand);
            entity.HasMany(e => e.Comments).WithOne(com => com.Order);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable(nameof(Client));
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Addresses).WithOne(co => co.Client);
            entity.HasMany(e => e.Comments).WithOne(com => com.Client);
            entity.HasMany(e => e.Orders);
            entity.Property(b => b.RegistrationDate).ValueGeneratedOnAdd().HasDefaultValueSql("getdate()");           
        });

        modelBuilder.Entity<Cleaner>(entity =>
        {
            entity.ToTable(nameof(Cleaner));
            entity.HasKey(e => e.Id);
            entity.HasMany(e => e.Districts).WithMany(co => co.Cleaners);
            entity.HasMany(e => e.Comments).WithOne(com => com.Cleaner);
            entity.HasMany(e => e.Orders).WithMany(o => o.CleanersBand);
        });

        modelBuilder.Entity<CleaningObject>(entity =>
        {
            entity.ToTable(nameof(CleaningObject));
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Client);
            entity.HasMany(e => e.Orders).WithOne(o => o.CleaningObject);
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
            entity.HasMany(e => e.Services);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.ToTable(nameof(Service));
        });
    }
}