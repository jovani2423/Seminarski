using System.ComponentModel.DataAnnotations;

namespace RentACar.Presentation.ViewModels
{
    public class CarViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka automobila je obavezna.")]
        [StringLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model automobila je obavezan.")]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "Unesite ispravnu godinu.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Registracija je obavezna.")]
        [StringLength(20)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "99999999.99",
            ErrorMessage = "Cena po danu mora biti veća od nule.")]
        public decimal PricePerDay { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }
    }
}