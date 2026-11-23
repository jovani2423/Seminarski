using System.ComponentModel.DataAnnotations;

namespace RentACar.Presentation.ViewModels
{
    public class BranchViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv poslovnice je obavezan.")]
        [StringLength(100, ErrorMessage = "Naziv može imati najviše 100 karaktera.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adresa poslovnice je obavezna.")]
        [StringLength(200, ErrorMessage = "Adresa može imati najviše 200 karaktera.")]
        public string Address { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Grad može imati najviše 50 karaktera.")]
        public string? City { get; set; }

        [StringLength(20, ErrorMessage = "Broj telefona može imati najviše 20 karaktera.")]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public int NumberOfCars { get; set; }
    }
}
