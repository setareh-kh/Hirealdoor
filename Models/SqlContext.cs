using Microsoft.EntityFrameworkCore;

namespace Hirealdoor.Models;

public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }
    public DbSet<User> Users => Set<User>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Office> Offices => Set<Office>();
    public DbSet<Province> Provinces => Set<Province>();
    public DbSet<ServedArea> ServedAreas => Set<ServedArea>();
    public DbSet<Language> Languages => Set<Language>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
         {
             e.HasKey(x => x.Id);
             e.Property(x => x.Email).IsRequired();
         });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(x => x.Id);
            // Shared Primary Key
            entity.HasOne(x => x.User)
                .WithOne(x => x.Person)
                .HasForeignKey<Person>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Office>(entity =>
        {
            entity.HasKey(x => x.Id);
             // Shared Primary Key
            entity.HasOne(x => x.User)
                .WithOne(x => x.Office)
                .HasForeignKey<Office>(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Person -> Office
        modelBuilder.Entity<Person>()
            .HasOne(p => p.Office)
            .WithMany(o => o.Employees)
            .HasForeignKey(p => p.OfficeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Office -> Province
        modelBuilder.Entity<Office>()
            .HasOne(o => o.City)
            .WithMany()
            .HasForeignKey(p => p.CityId)
            .OnDelete(DeleteBehavior.Restrict);
        //person -> ServedArea<-Province
        modelBuilder.Entity<ServedArea>()
        .HasKey(pp => new
        {
            pp.PersonId,
            pp.ProvinceId
        });

        modelBuilder.Entity<ServedArea>()
            .HasOne(pp => pp.Person)
            .WithMany(p => p.ServedAreas)
            .HasForeignKey(pp => pp.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServedArea>()
            .HasOne(sa => sa.Province)
            .WithMany()
            .HasForeignKey(sa => sa.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Person>()
        .HasMany(p => p.SpeaksLanguages)
        .WithMany(l => l.Persons)
        .UsingEntity("SpeaksLanguages");
    }
}