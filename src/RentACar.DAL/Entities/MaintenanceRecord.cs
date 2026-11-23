
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentACar.DAL.Entities
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; }

        public Car Car { get; set; } = null!;

        [Required]
        public DateTime MaintenanceDate { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        [Range(typeof(decimal), "0", "99999999.99")]
        public decimal Cost { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Planned";
    }
}
