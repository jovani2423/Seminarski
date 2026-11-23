using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RentACar.Presentation.ViewModels
{
    public class MaintenanceRecordViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Izaberite automobil.")]
        public int CarId { get; set; }

        public string CarBrand { get; set; } = string.Empty;

        public string CarModel { get; set; } = string.Empty;

        public string RegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Unesite datum održavanja.")]
        [DataType(DataType.Date)]
        public DateTime MaintenanceDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Unesite opis održavanja.")]
        [StringLength(1000, ErrorMessage = "Opis može imati najviše 1000 karaktera.")]
        public string Description { get; set; } = string.Empty;

        [Range(typeof(decimal), "0", "99999999.99",
            ErrorMessage = "Cena mora biti pozitivna vrednost.")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Izaberite status održavanja.")]
        public string Status { get; set; } = "Planned";

        public List<SelectListItem> Cars { get; set; } = new();

        public List<SelectListItem> Statuses { get; set; } = new();
    }
}
