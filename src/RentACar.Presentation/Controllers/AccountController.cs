using Microsoft.AspNetCore.Mvc;
using RentACar.BLL.DTOs;
using RentACar.BLL.Services;
using RentACar.Presentation.ViewModels;

namespace RentACar.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new RegisterDto
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                Email = viewModel.Email,
                Password = viewModel.Password,
                ConfirmPassword = viewModel.ConfirmPassword
            };

            var result = await _accountService.RegisterAsync(dto);

            if (result.Succeeded)
            {
                await _accountService.LoginAsync(new LoginDto
                {
                    Email = viewModel.Email,
                    Password = viewModel.Password,
                    RememberMe = false
                });

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new LoginDto
            {
                Email = viewModel.Email,
                Password = viewModel.Password,
                RememberMe = viewModel.RememberMe
            };

            var loginResult = await _accountService.LoginAsync(new LoginDto
            {
                Email = viewModel.Email,
                Password = viewModel.Password,
                RememberMe = false
            });

            if (loginResult.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError(
                string.Empty,
                "Registracija je uspešna, ali automatsko prijavljivanje nije uspelo.");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
