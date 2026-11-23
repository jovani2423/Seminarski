
using System.ComponentModel.DataAnnotations;

namespace RentACar.DAL.Entities
{
    public class CarCategory
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Car> Cars { get; set; }
            = new List<Car>();
    }
}
