using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.BLL.DTOs;
using RentACar.BLL.Services;
using RentACar.Presentation.ViewModels;

namespace RentACar.Presentation.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MaintenanceRecordController : Controller
    {
        private readonly MaintenanceRecordService _maintenanceService;
        private readonly CarService _carService;

        public MaintenanceRecordController(
            MaintenanceRecordService maintenanceService,
            CarService carService)
        {
            _maintenanceService = maintenanceService;
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var records = await _maintenanceService.GetAllAsync();

            var viewModels = records.Select(record =>
                MapToViewModel(record)).ToList();

            return View(viewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new MaintenanceRecordViewModel
            {
                MaintenanceDate = DateTime.Today,
                Status = "Planned"
            };

            await PopulateDropdownsAsync(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            MaintenanceRecordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(viewModel);
                return View(viewModel);
            }

            var dto = MapToDto(viewModel);

            bool success = await _maintenanceService.AddAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Podaci nisu ispravni ili održavanje nije moguće dodati.");

                await PopulateDropdownsAsync(viewModel);
                return View(viewModel);
            }

            TempData["SuccessMessage"] =
                "Evidencija održavanja je uspešno dodata.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var record = await _maintenanceService.GetByIdAsync(id);

            if (record == null)
                return NotFound();

            var viewModel = MapToViewModel(record);

            await PopulateDropdownsAsync(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            MaintenanceRecordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(viewModel);
                return View(viewModel);
            }

            var dto = MapToDto(viewModel);

            bool success = await _maintenanceService.UpdateAsync(dto);

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Evidencija ne postoji ili podaci nisu ispravni.");

                await PopulateDropdownsAsync(viewModel);
                return View(viewModel);
            }

            TempData["SuccessMessage"] =
                "Evidencija održavanja je uspešno izmenjena.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool success = await _maintenanceService.DeleteAsync(id);

            if (!success)
            {
                TempData["ErrorMessage"] =
                    "Evidencija održavanja nije pronađena.";

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] =
                "Evidencija održavanja je uspešno obrisana.";

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(
            MaintenanceRecordViewModel viewModel)
        {
            var cars = await _carService.SearchCarsAsync(
                new CarSearchDto());

            var carItems = cars
                .Select(car => new SelectListItem
                {
                    Value = car.Id.ToString(),
                    Text = $"{car.Brand} {car.Model} - {car.RegistrationNumber}"
                })
                .ToList();

            if (viewModel.CarId > 0 &&
                !carItems.Any(c =>
                    c.Value == viewModel.CarId.ToString()))
            {
                var existingCar = await _carService.GetCarByIdAsync(
                    viewModel.CarId);

                if (existingCar != null)
                {
                    carItems.Add(new SelectListItem
                    {
                        Value = existingCar.Id.ToString(),
                        Text = $"{existingCar.Brand} {existingCar.Model} - {existingCar.RegistrationNumber}"
                    });
                }
            }

            viewModel.Cars = carItems;

            viewModel.Statuses = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "Planned",
                    Text = "Planirano"
                },
                new SelectListItem
                {
                    Value = "InProgress",
                    Text = "U toku"
                },
                new SelectListItem
                {
                    Value = "Completed",
                    Text = "Završeno"
                }
            };
        }

        private static MaintenanceRecordViewModel MapToViewModel(
            MaintenanceRecordDto dto)
        {
            return new MaintenanceRecordViewModel
            {
                Id = dto.Id,
                CarId = dto.CarId,
                CarBrand = dto.CarBrand,
                CarModel = dto.CarModel,
                RegistrationNumber = dto.RegistrationNumber,
                MaintenanceDate = dto.MaintenanceDate,
                Description = dto.Description,
                Cost = dto.Cost,
                Status = dto.Status
            };
        }

        private static MaintenanceRecordDto MapToDto(
            MaintenanceRecordViewModel viewModel)
        {
            return new MaintenanceRecordDto
            {
                Id = viewModel.Id,
                CarId = viewModel.CarId,
                MaintenanceDate = viewModel.MaintenanceDate,
                Description = viewModel.Description,
                Cost = viewModel.Cost,
                Status = viewModel.Status
            };
        }
    }
}
