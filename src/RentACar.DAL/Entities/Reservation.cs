using System.ComponentModel.DataAnnotations;
using RentACar.DAL.Identity;

namespace RentACar.DAL.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }

        public Car Car { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}