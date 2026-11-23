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
            if (dto == null)
                return IdentityResult.Failed(
                    new IdentityError { Description = "Podaci za registraciju nisu ispravni." });

            if (string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password) ||
                string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = "Sva polja su obavezna." });
            }

            if (dto.FirstName.Trim().Length > 50 ||
                dto.LastName.Trim().Length > 50)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = "Ime i prezime mogu imati najviše 50 karaktera." });
            }

            if (dto.Email.Trim().Length > 256)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = "Email adresa je predugačka." });
            }

            if (dto.Password != dto.ConfirmPassword)
            {
                return IdentityResult.Failed(
                    new IdentityError { Description = "Lozinke se ne poklapaju." });
            }

            var user = new ApplicationUser
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim(),
                UserName = dto.Email.Trim(),
                RegistrationDate = DateTime.Now
            };

            return await _userManager.CreateAsync(user, dto.Password);
        }

        public async Task<SignInResult> LoginAsync(LoginDto dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return SignInResult.Failed;
            }

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
