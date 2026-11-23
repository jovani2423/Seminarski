using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.BLL.DTOs;
using RentACar.BLL.Services;

namespace RentACar.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CarCategoryController : Controller
    {
        private readonly CarCategoryService _categoryService;

        public CarCategoryController(
            CarCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories =
                await _categoryService.GetAllAsync();

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CarCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            bool success = await _categoryService.AddAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(
                    nameof(dto.Name),
                    "Naziv kategorije već postoji ili podaci nisu ispravni.");

                return View(dto);
            }

            TempData["SuccessMessage"] =
                "Kategorija je uspešno dodata.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category =
                await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            CarCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            bool success = await _categoryService.UpdateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(
                    nameof(dto.Name),
                    "Naziv kategorije već postoji ili podaci nisu ispravni.");

                return View(dto);
            }

            TempData["SuccessMessage"] =
                "Kategorija je uspešno izmenjena.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await _categoryService.DeleteAsync(id);

            if (!success)
            {
                TempData["ErrorMessage"] =
                    "Kategorija ne postoji ili je dodeljena nekom automobilu.";

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] =
                "Kategorija je uspešno obrisana.";

            return RedirectToAction(nameof(Index));
        }
    }
}
