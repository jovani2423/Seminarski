using System.ComponentModel.DataAnnotations;

namespace RentACar.BLL.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Ime je obavezno.")]
        [StringLength(50, ErrorMessage = "Ime može imati najviše 50 karaktera.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prezime je obavezno.")]
        [StringLength(50, ErrorMessage = "Prezime može imati najviše 50 karaktera.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email adresa je obavezna.")]
        [EmailAddress(ErrorMessage = "Email adresa nije ispravna.")]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [MinLength(8, ErrorMessage = "Lozinka mora imati najmanje 8 karaktera.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potvrda lozinke je obavezna.")]
        [Compare("Password", ErrorMessage = "Lozinke se ne poklapaju.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
