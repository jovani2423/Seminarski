using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.BLL.DTOs;
using RentACar.BLL.Services;
using RentACar.Presentation.ViewModels;

namespace RentACar.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BranchController : Controller
    {
        private readonly BranchService _branchService;

        public BranchController(BranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var branches = await _branchService.GetAllAsync();

            var viewModels = branches.Select(b => new BranchViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                City = b.City,
                PhoneNumber = b.PhoneNumber,
                IsActive = b.IsActive,
                NumberOfCars = b.NumberOfCars
            }).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new BranchViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BranchViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new BranchDto
            {
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                PhoneNumber = model.PhoneNumber,
                IsActive = model.IsActive
            };

            bool success = await _branchService.AddAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(
                    nameof(model.Name),
                    "Poslovnica sa ovim nazivom već postoji ili podaci nisu ispravni.");

                return View(model);
            }

            TempData["SuccessMessage"] =
                "Poslovnica je uspešno dodata.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var branch = await _branchService.GetByIdAsync(id);

            if (branch == null)
                return NotFound();

            var model = new BranchViewModel
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                City = branch.City,
                PhoneNumber = branch.PhoneNumber,
                IsActive = branch.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BranchViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new BranchDto
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                City = model.City,
                PhoneNumber = model.PhoneNumber,
                IsActive = model.IsActive
            };

            bool success = await _branchService.UpdateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(
                    nameof(model.Name),
                    "Poslovnica ne postoji, naziv je zauzet ili podaci nisu ispravni.");

                return View(model);
            }

            TempData["SuccessMessage"] =
                "Podaci o poslovnici su uspešno izmenjeni.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await _branchService.DeleteAsync(id);

            if (!success)
            {
                TempData["ErrorMessage"] =
                    "Poslovnica ne postoji ili je dodeljena nekom automobilu. U tom slučaju je deaktiviraj.";

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] =
                "Poslovnica je uspešno obrisana.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            bool success = await _branchService.DeactivateAsync(id);

            if (!success)
            {
                TempData["ErrorMessage"] =
                    "Poslovnica nije pronađena ili nije moguće izvršiti deaktivaciju.";

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] =
                "Poslovnica je uspešno deaktivirana.";

            return RedirectToAction(nameof(Index));
        }

    }
}
