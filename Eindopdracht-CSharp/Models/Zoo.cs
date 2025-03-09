using System.ComponentModel.DataAnnotations;

namespace Eindopdracht_CSharp.Models;

public class Zoo
{
    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; }

    public List<Enclosure> Enclosures { get; set; } = new List<Enclosure>();

    public Zoo()
    {
    }
}