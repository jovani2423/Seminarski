using Microsoft.AspNetCore.Identity;
using RentACar.BLL.DTOs;
using RentACar.DAL.Identity;

namespace RentACar.BLL.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                UserName = dto.Email.Trim(),
                RegistrationDate = DateTime.Now
            };

            return await _userManager.CreateAsync(
                user, dto.Password);
        }

        public async Task<SignInResult> LoginAsync(LoginDto dto)
        {
            return await _signInManager.PasswordSignInAsync(
                dto.Email.Trim(),
                dto.Password,
                dto.RememberMe,
                lockoutOnFailure: true);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
