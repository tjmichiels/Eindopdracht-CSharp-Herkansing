using Bogus;
using Eindopdracht_CSharp.Enums;
using Eindopdracht_CSharp.Models;
using Microsoft.EntityFrameworkCore;

namespace Eindopdracht_CSharp.Data;

public class ZooDbContext : DbContext
{
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Enclosure> Enclosures { get; set; }
    public DbSet<Zoo> Zoos { get; set; }

    public ZooDbContext(DbContextOptions<ZooDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(warnings =>
            warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Primary keys
        modelBuilder.Entity<Animal>()
            .HasKey(a => a.Id);

        // Animal -> Category
        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Category)
            .WithMany(c => c.Animals)
            .HasForeignKey(a => a.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Animal -> Enclosure
        modelBuilder.Entity<Animal>()
            .HasOne(a => a.Enclosure)
            .WithMany(e => e.Animals)
            .HasForeignKey(a => a.EnclosureId)
            .OnDelete(DeleteBehavior.SetNull);

        // Enclosure -> Zoo
        modelBuilder.Entity<Enclosure>()
            .HasOne(e => e.Zoo)
            .WithMany(z => z.Enclosures)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        // Seeding-methode aanroepen
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
// Zoo seeding
        var zooFaker = new Faker<Zoo>()
            .RuleFor(z => z.Id, f => f.IndexFaker + 1)
            .RuleFor(z => z.Name, f => f.Company.CompanyName());
        var zoos = zooFaker.Generate(5);
        modelBuilder.Entity<Zoo>().HasData(zoos);
        
        // ---- Category seeding
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Mammals" },
            new Category { Id = 2, Name = "Birds" },
            new Category { Id = 3, Name = "Reptiles" },
            new Category { Id = 4, Name = "Aquatic Animals" },
            new Category { Id = 5, Name = "Insects" }
        };
        modelBuilder.Entity<Category>().HasData(categories);
        
        // ---- Enclosure seeding
        var enclosureFaker = new Faker<Enclosure>()
            .RuleFor(e => e.Id, f => f.IndexFaker + 1)
            .RuleFor(e => e.Name, f => f.Commerce.Department())
            .RuleFor(e => e.Climate, f => f.PickRandom<Climate>())
            .RuleFor(e => e.HabitatType, f => f.PickRandom<HabitatType>())
            .RuleFor(e => e.SecurityLevel, f => f.PickRandom<SecurityLevel>())
            .RuleFor(e => e.Size, f => f.Random.Int(100, 1000))
            .RuleFor(e => e.ZooId, f => f.PickRandom(zoos.Select(z => z.Id)));
        
        var enclosures = enclosureFaker.Generate(15);
        modelBuilder.Entity<Enclosure>().HasData(enclosures);
        
        // Animal seeding
        var animalFaker = new Faker<Animal>()
            .RuleFor(a => a.Id, f => f.IndexFaker + 1)
            .RuleFor(a => a.Name, f => f.Name.FirstName())
            .RuleFor(a => a.Species, f => f.PickRandom<Species>())
            .RuleFor(a => a.Size, f => f.PickRandom<AnimalSize>())
            .RuleFor(a => a.DietaryClass, f => f.PickRandom<DietaryClass>())
            .RuleFor(a => a.ActivityPattern, f => f.PickRandom<ActivityPattern>())
            .RuleFor(a => a.SpaceRequirement, f => f.Random.Double(1, 20))
            .RuleFor(a => a.SecurityRequirement, f => f.PickRandom<SecurityLevel>())
            .RuleFor(a => a.CategoryId, f => f.PickRandom(categories.Select(c => c.Id)))
            .RuleFor(a => a.EnclosureId, f => f.PickRandom(enclosures.Select(e => e.Id)));

        var animals = animalFaker.Generate(10);
        modelBuilder.Entity<Animal>().HasData(animals);
    }
}