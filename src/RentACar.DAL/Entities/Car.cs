using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentACar.DAL.Entities
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2100)]
        public int Year { get; set; }

        [Required]
        [MaxLength(20)]
        public string RegistrationNumber { get; set; }
            = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        [Range(typeof(decimal), "0.01", "99999999.99")]
        public decimal PricePerDay { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public int? CarCategoryId { get; set; }

        public CarCategory? CarCategory { get; set; }

        public int? BranchId { get; set; }

        public Branch? Branch { get; set; }

        public ICollection<MaintenanceRecord> MaintenanceRecords
        { get; set; } = new List<MaintenanceRecord>();
    }
}
