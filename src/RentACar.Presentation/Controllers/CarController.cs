using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.BLL.DTOs;
using RentACar.BLL.Services;
using RentACar.Presentation.ViewModels;

namespace RentACar.Presentation.Controllers
{
    public class CarController : Controller
    {
        private readonly CarService _carService;

        public CarController(CarService carService)
        {
            _carService = carService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchTerm,
            decimal? minPrice,
            decimal? maxPrice,
            int? minYear,
            int? maxYear)
        {
            var searchDto = new CarSearchDto
            {
                SearchTerm = searchTerm,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinYear = minYear,
                MaxYear = maxYear
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
                ImageUrl = car.ImageUrl
            }).ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.MinYear = minYear;
            ViewBag.MaxYear = maxYear;

            return View(viewModels);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarViewModel carViewModel)
        {
            if (!ModelState.IsValid)
                return View(carViewModel);

            var carDto = new CarDto
            {
                Brand = carViewModel.Brand,
                Model = carViewModel.Model,
                Year = carViewModel.Year,
                RegistrationNumber = carViewModel.RegistrationNumber,
                PricePerDay = carViewModel.PricePerDay,
                Description = carViewModel.Description,
                ImageUrl = carViewModel.ImageUrl
            };

            bool success = await _carService.AddCarAsync(carDto);

            if (!success)
            {
                ModelState.AddModelError(
                    nameof(carViewModel.RegistrationNumber),
                    "Registracija već postoji ili podaci nisu ispravni.");

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
            {
                return NotFound();
            }

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
                ImageUrl = car.ImageUrl
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarViewModel carViewModel)
        {
            if (!ModelState.IsValid)
                return View(carViewModel);

            var carDto = new CarDto
            {
                Id = carViewModel.Id,
                Brand = carViewModel.Brand,
                Model = carViewModel.Model,
                Year = carViewModel.Year,
                RegistrationNumber = carViewModel.RegistrationNumber,
                PricePerDay = carViewModel.PricePerDay,
                Description = carViewModel.Description,
                ImageUrl = carViewModel.ImageUrl
            };

            bool success = await _carService.UpdateCarAsync(carDto);

            if (!success)
            {
                ModelState.AddModelError(
                    nameof(carViewModel.RegistrationNumber),
                    "Registracija već postoji ili podaci nisu ispravni.");

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
                ImageUrl = car.ImageUrl
            };

            return View(viewModel);
        }
    }
}