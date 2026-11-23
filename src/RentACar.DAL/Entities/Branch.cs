
using System.ComponentModel.DataAnnotations;

namespace RentACar.DAL.Entities
{
    public class Branch
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Car> Cars { get; set; }
            = new List<Car>();
    }
}

