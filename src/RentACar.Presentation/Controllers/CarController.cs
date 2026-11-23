using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.BLL.DTOs;
using RentACar.BLL.Services;
using RentACar.Presentation.ViewModels;

namespace RentACar.Presentation.Controllers
{
    public class CarController : Controller
    {
        private readonly CarService _carService;
        private readonly CarCategoryService _categoryService;
        private readonly BranchService _branchService;

        public CarController(
            CarService carService,
            CarCategoryService categoryService,
            BranchService branchService)
        {
            _carService = carService;
            _categoryService = categoryService;
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchTerm,
            decimal? minPrice,
            decimal? maxPrice,
            int? minYear,
            int? maxYear,
            int? carCategoryId,
            int? branchId)
        {
            var searchDto = new CarSearchDto
            {
                SearchTerm = searchTerm,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinYear = minYear,
                MaxYear = maxYear,
                CarCategoryId = carCategoryId,
                BranchId = branchId
            };

            var cars = await _carService.SearchCarsAsync(searchDto);

            var viewModels = cars.Select(car => new CarViewModel
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                RegistrationNumber = car.RegistrationNumber,
                PricePerDay = car.PricePerDay,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                CarCategoryId = car.CarCategoryId,
                CarCategoryName = car.CarCategoryName,
                BranchId = car.BranchId,
                BranchName = car.BranchName
            }).ToList();

            var categories = await _categoryService.GetAllAsync();

            ViewBag.Categories = categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            var branches = await _branchService.GetActiveAsync();

            ViewBag.Branches = branches
                .OrderBy(b => b.Name)
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.MinYear = minYear;
            ViewBag.MaxYear = maxYear;
            ViewBag.CarCategoryId = carCategoryId;
            ViewBag.BranchId = branchId;

            return View(viewModels);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CarViewModel();

            await PopulateDropdownsAsync(viewModel);

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarViewModel carViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(carViewModel);
                return View(carViewModel);
            }

            var carDto = new CarDto
            {
                Brand = carViewModel.Brand,
                Model = carViewModel.Model,
                Year = carViewModel.Year,
                RegistrationNumber = carViewModel.RegistrationNumber,
                PricePerDay = carViewModel.PricePerDay,
                Description = carViewModel.Description,
                ImageUrl = carViewModel.ImageUrl,
                CarCategoryId = carViewModel.CarCategoryId,
                BranchId = carViewModel.BranchId
            };

            bool success = await _carService.AddCarAsync(carDto);

            if (!success)
            {
                ModelState.AddModelError(
                    nameof(carViewModel.RegistrationNumber),
                    "Registracija već postoji ili kategorija/poslovnica nije ispravna.");

                await PopulateDropdownsAsync(carViewModel);
                return View(carViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            bool success = await _carService.DeactivateCarAsync(id);

            if (!success)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);

            if (car == null)
                return NotFound();

            var viewModel = new CarViewModel
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                RegistrationNumber = car.RegistrationNumber,
                PricePerDay = car.PricePerDay,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                CarCategoryId = car.CarCategoryId,
                CarCategoryName = car.CarCategoryName,
                BranchId = car.BranchId,
                BranchName = car.BranchName
            };

            await PopulateDropdownsAsync(viewModel);

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarViewModel carViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(carViewModel);
                return View(carViewModel);
            }

            var carDto = new CarDto
            {
                Id = carViewModel.Id,
                Brand = carViewModel.Brand,
                Model = carViewModel.Model,
                Year = carViewModel.Year,
                RegistrationNumber = carViewModel.RegistrationNumber,
                PricePerDay = carViewModel.PricePerDay,
                Description = carViewModel.Description,
                ImageUrl = carViewModel.ImageUrl,
                CarCategoryId = carViewModel.CarCategoryId,
                BranchId = carViewModel.BranchId
            };

            bool success = await _carService.UpdateCarAsync(carDto);

            if (!success)
            {
                ModelState.AddModelError(
                    nameof(carViewModel.RegistrationNumber),
                    "Registracija već postoji ili kategorija/poslovnica nije ispravna.");

                await PopulateDropdownsAsync(carViewModel);
                return View(carViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);

            if (car == null)
                return NotFound();

            var viewModel = new CarViewModel
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                RegistrationNumber = car.RegistrationNumber,
                PricePerDay = car.PricePerDay,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                CarCategoryId = car.CarCategoryId,
                CarCategoryName = car.CarCategoryName,
                BranchId = car.BranchId,
                BranchName = car.BranchName
            };

            return View(viewModel);
        }

        private async Task PopulateDropdownsAsync(
            CarViewModel viewModel)
        {
            var categories = await _categoryService.GetAllAsync();

            viewModel.Categories = categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();

            var branches = await _branchService.GetActiveAsync();

            viewModel.Branches = branches
                .OrderBy(b => b.Name)
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .ToList();
        }
    }
}
