using EzLib.Data;
using EzLib.Models;
using EzLib.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzLib.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly EzLibContext _context;
        private readonly ICategoryService _categoryService;

        public CategoriesController(EzLibContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync();
                return View(categories);
            }
            catch (InvalidOperationException ex)
            {
                return Problem(ex.Message);
            }
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create   // No seperate service needed
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (!await _categoryService.IsCategoryNameUnique(category))
                {
                    ModelState.AddModelError(string.Empty, "Category name must be unique.");
                    return View(category);
                }

                await _categoryService.CreateCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Category == null)
            {
                return NotFound();
            }

            var category = await _categoryService.GetCategoryAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!await _categoryService.IsCategoryNameUnique(category))
                {
                    ModelState.AddModelError(string.Empty, "Category name must be unique.");
                    return View(category);
                }

                bool updateResult = await _categoryService.UpdateCategoryAsync(category);

                if (!updateResult)
                {
                    if (!_categoryService.CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw new Exception("An error occurred while updating the category.");
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // klar

        // GET: Categories/Delete/5     // funkar denna tro?
        public async Task<IActionResult> Delete(int? id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5        // denna kvar bara?
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await _categoryService.CanDeleteCategoryAsync(id))
            {
                ModelState.AddModelError(string.Empty, "Category is referenced by one or more library items and cannot be deleted.");
                return View(nameof(Delete), await _context.Category.FindAsync(id));
            }

            if (_context.Category == null)
            {
                return Problem("Entity set 'EzLibContext.Category'  is null.");
            }
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
