using System.ComponentModel.DataAnnotations;
using Eindopdracht_CSharp.Enums;

namespace Eindopdracht_CSharp.Models;

public class Animal
{
    [Key] public int Id { get; set; }

    [MaxLength(100)] [Required] public string Name { get; set; }

    [Required] public Species Species { get; set; }

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public int? EnclosureId { get; set; }
    public Enclosure? Enclosure { get; set; }
    public AnimalSize Size { get; set; }
    public DietaryClass DietaryClass { get; set; }
    public ActivityPattern ActivityPattern { get; set; }
    public HashSet<Animal> Prey { get; set; } = new HashSet<Animal>();
    public double SpaceRequirement { get; set; } // square meters per animal
    public SecurityLevel SecurityRequirement { get; set; }

    public Animal()
    {
    }

    public string Sunrise()
    {
        // Actie Sunrise die aangeeft of het dier wakker wordt of gaan slapen of altijd actief is.
        return string.Empty;
    }

    public string Sunset()
    {
        // Actie Sunset die aangeeft of het dier wakker wordt of gaan slapen of altijd actief is.
        return string.Empty;
    }
}