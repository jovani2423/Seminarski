using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RentACar.Presentation.ViewModels
{
    public class ReservationViewModel
    {
        [Range(1, int.MaxValue, ErrorMessage = "Morate izabrati automobil.")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Datum preuzimanja je obavezan.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Datum vraćanja je obavezan.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);

        public List<SelectListItem> AvailableCars { get; set; } = new();
    }
}
