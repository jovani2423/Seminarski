using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public int? CarCategoryId { get; set; }

        public string? CarCategoryName { get; set; }

        public int? BranchId { get; set; }

        public string? BranchName { get; set; }

        public List<SelectListItem> Categories { get; set; }
            = new List<SelectListItem>();

        public List<SelectListItem> Branches { get; set; }
            = new List<SelectListItem>();
    }
}
