using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using System.ComponentModel.DataAnnotations;
using Eindopdracht_CSharp.Enums;
using Microsoft.EntityFrameworkCore;

namespace Eindopdracht_CSharp.Models;

[Index(nameof(Id), nameof(Name), IsUnique = true)]
public class Enclosure
{
    [Key] public int Id { get; set; }
    [Required] public string Name { get; set; }
    public int? ZooId { get; set; }
    public Zoo? Zoo { get; set; }
    public List<Animal> Animals { get; set; } = new();
    [Required] public Climate Climate { get; set; }
    [Required] public HabitatType HabitatType { get; set; }
    [Required] public SecurityLevel SecurityLevel { get; set; }
    public double Size { get; set; } // square meters


    public Enclosure()
    {
    }

    public bool CheckConstraints(out List<string> meldingen)
    {
        meldingen = new List<string>();

        if (string.IsNullOrWhiteSpace(Name))
        {
            meldingen.Add("Name is required.");
        }

        if (Climate == 0)
        {
            meldingen.Add("Climate is required.");
        }

        if (HabitatType == 0)
        {
            meldingen.Add("Habitat is required.");
        }

        if (SecurityLevel == 0)
        {
            meldingen.Add("Security level is required.");
        }

        if (Size <= 0)
        {
            meldingen.Add("Size must be greater than 0.");
        }

        if (Animals == null || Animals.Count == 0)
        {
            meldingen.Add("Add one animal to the list.");
        }

        return meldingen.Count == 0;
    }
    /*
      var errors = new List<string>();
        bool isValid = enclosure.CheckConstraints(out errors);

        if (!isValid)
        {
            Console.WriteLine("Enclosure constraints not met:");
            foreach (var error in errors)
            {
                Console.WriteLine($"- {error}");
            }
        }
        else
        {
            Console.WriteLine("All constraints are met. The enclosure is valid.");
        }

     */
}