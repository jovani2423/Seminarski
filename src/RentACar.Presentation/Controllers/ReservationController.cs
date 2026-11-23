using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RentACar.BLL.DTOs;
using RentACar.BLL.Services;
using RentACar.Presentation.ViewModels;
using System.Security.Claims;

namespace RentACar.Presentation.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ReservationService _reservationService;
        private readonly CarService _carService;

        public ReservationController(
            ReservationService reservationService,
            CarService carService)
        {
            _reservationService = reservationService;
            _carService = carService;
        }

        private async Task LoadCarsAsync()
        {
            var cars = await _carService.GetAllCarsAsync();

            ViewBag.AvailableCars = cars.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = $"{c.Brand} {c.Model} ({c.Year}) - {c.PricePerDay:N2} RSD"
            }).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? carId)
        {
            await LoadCarsAsync();

            var viewModel = new ReservationViewModel();

            if (carId.HasValue)
                viewModel.CarId = carId.Value;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            ReservationViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await LoadCarsAsync();
                return View(viewModel);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var dto = new ReservationDto
            {
                CarId = viewModel.CarId,
                UserId = userId,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate
            };

            var result =
                await _reservationService.CreateReservationAsync(dto);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                await LoadCarsAsync();
                return View(viewModel);
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(nameof(MyReservations));
        }

        [HttpGet]
        public async Task<IActionResult> MyReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var reservations =
                await _reservationService.GetUserReservationsAsync(userId);

            return View(reservations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var result = await _reservationService.CancelReservationAsync(
                id,
                userId);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction(nameof(MyReservations));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllReservations()
        {
            var reservations =
                await _reservationService.GetAllReservationsAsync();

            return View(reservations);
        }
    }
}
