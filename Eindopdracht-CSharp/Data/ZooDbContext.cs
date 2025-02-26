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
            .HasOne<Zoo>()
            .WithMany(z => z.Enclosures)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        // ZOO SEEDING
        // Primary Keys
        modelBuilder.Entity<Zoo>().HasKey(z => z.Id);
        
        int zooId = 1;
        var zooFaker = new Faker<Zoo>()
            .RuleFor(z => z.Id, _ => zooId++)  // Ensure unique IDs
            .RuleFor(z => z.Name, f => f.Company.CompanyName());

        modelBuilder.Entity<Zoo>().HasData(zooFaker.Generate(5));  
        
        // Seeding-methode aanroepen
        SeedData(modelBuilder);
    }
    
     private void SeedData(ModelBuilder modelBuilder)
    {
        var faker = new Faker();

        // Seed Categories
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Mammals" },
            new Category { Id = 2, Name = "Birds" },
            new Category { Id = 3, Name = "Reptiles" },
            new Category { Id = 4, Name = "Aquatic Animals" },
            new Category { Id = 5, Name = "Insects" }
        };
        modelBuilder.Entity<Category>().HasData(categories);

        // Seed Enclosures
        var enclosures = new List<Enclosure>
        {
            new Enclosure { Id = 1, Name = "Savanna", Climate = Climate.Tropical, HabitatType = HabitatType.Grassland, SecurityLevel = SecurityLevel.High, Size = 500 },
            new Enclosure { Id = 2, Name = "Rainforest", Climate = Climate.Tropical, HabitatType = HabitatType.Forest, SecurityLevel = SecurityLevel.Medium, Size = 300 },
            new Enclosure { Id = 3, Name = "Arctic Zone", Climate = Climate.Arctic, HabitatType = HabitatType.Forest, SecurityLevel = SecurityLevel.High, Size = 400 }
        };
        modelBuilder.Entity<Enclosure>().HasData(enclosures);

        // Seed Animals using `Bogus`
        var animals = new List<Animal>();
        for (int i = 1; i <= 10; i++)
        {
            animals.Add(new Animal
            {
                Id = i,
                Name = faker.Name.FirstName(),
                Species = faker.PickRandom<Species>(),
                Size = faker.PickRandom<AnimalSize>(),
                DietaryClass = faker.PickRandom<DietaryClass>(),
                ActivityPattern = faker.PickRandom<ActivityPattern>(),
                SpaceRequirement = faker.Random.Double(1, 20),
                SecurityRequirement = faker.PickRandom<SecurityLevel>(),
                CategoryId = faker.PickRandom(categories.Select(c => (int?)c.Id)), 
                // Enclosure = faker.PickRandom(enclosures),
                EnclosureId = faker.PickRandom(enclosures.Select(e => (int?)e.Id))
            });
        }
        modelBuilder.Entity<Animal>().HasData(animals);
    }
}