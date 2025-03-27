using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eindopdracht_CSharp.Data;
using Eindopdracht_CSharp.Enums;
using Eindopdracht_CSharp.Models;

namespace Eindopdracht_CSharp.Controllers
{
    public class AnimalController : Controller
    {
        private readonly ZooDbContext _context;

        public AnimalController(ZooDbContext context)
        {
            _context = context;
        }

        // GET: Animal
        public async Task<IActionResult> Index()
        {
            // return View(await _context.Animals.ToListAsync());
            return View(await _context.Animals
                .Include(a => a.Enclosure)
                .ThenInclude(e => e.Zoo)
                .Include(a => a.Category)
                .ToListAsync());
        }

        // GET: Animal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.Enclosure)
                .ThenInclude(e => e.Zoo)
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animal/Create
        public IActionResult Create()
        {
            // ViewBag.Enclosures = new SelectList(_context.Enclosures, "Id", "Name");
            ViewBag.Enclosures = _context.Enclosures
                .Include(e => e.Zoo)
                .AsEnumerable()
                .OrderBy(e => e.Zoo?.Name) // sorteren op dierentuinnaam (nulls eerst)
                .ThenBy(e => e.Name)       // en dan eventueel op enclosure naam
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = $"{e.Zoo?.Name}: {e.Name}"
                }).ToList();
            
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.DietaryClassList = new SelectList(Enum.GetValues(typeof(DietaryClass)));
            ViewBag.ActivityPatternList = new SelectList(Enum.GetValues(typeof(ActivityPattern)));
            ViewBag.SecurityRequirementList = new SelectList(Enum.GetValues(typeof(SecurityLevel)));
            ViewBag.SpeciesList = new SelectList(Enum.GetValues(typeof(Species)));
            ViewBag.SizeList = new SelectList(Enum.GetValues(typeof(AnimalSize)));

            return View();
        }

        // POST: Animal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Species,Size,DietaryClass,ActivityPattern,SpaceRequirement,SecurityRequirement,EnclosureId")]
            Animal animal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Enclosure ID & naam
            ViewBag.Enclosures = new SelectList(_context.Enclosures, "Id", "Name");

            // Category ID & naam
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(animal);
        }

        // GET: Animal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            // Benodigde viewbags
            ViewBag.DietaryClassList = new SelectList(Enum.GetValues(typeof(DietaryClass)));
            ViewBag.ActivityPatternList = new SelectList(Enum.GetValues(typeof(ActivityPattern)));
            ViewBag.SecurityRequirementList = new SelectList(Enum.GetValues(typeof(SecurityLevel)));
            ViewBag.SpeciesList = new SelectList(Enum.GetValues(typeof(Species)));
            ViewBag.SizeList = new SelectList(Enum.GetValues(typeof(AnimalSize)));
            ViewBag.Enclosures = new SelectList(_context.Enclosures, "Id", "Name", animal.EnclosureId);
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", animal.CategoryId);

            return View(animal);
        }

        // POST: Animal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Name,Species,Size,DietaryClass,ActivityPattern,SpaceRequirement,SecurityRequirement")]
            Animal animal)
        {
            if (id != animal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            // Viewbags
            ViewBag.DietaryClassList = new SelectList(Enum.GetValues(typeof(DietaryClass)));
            ViewBag.ActivityPatternList = new SelectList(Enum.GetValues(typeof(ActivityPattern)));
            ViewBag.SecurityRequirementList = new SelectList(Enum.GetValues(typeof(SecurityLevel)));
            ViewBag.SpeciesList = new SelectList(Enum.GetValues(typeof(Species)));
            ViewBag.SizeList = new SelectList(Enum.GetValues(typeof(AnimalSize)));
            ViewBag.Enclosures = new SelectList(_context.Enclosures, "Id", "Name", animal.EnclosureId);
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", animal.CategoryId);
            return View(animal);
        }

        // GET: Animal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animals
                .Include(a => a.Enclosure)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal != null)
            {
                _context.Animals.Remove(animal);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id)
        {
            return _context.Animals.Any(e => e.Id == id);
        }
    }
}