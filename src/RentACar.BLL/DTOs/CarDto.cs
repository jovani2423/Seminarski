namespace RentACar.BLL.DTOs
{
    public class CarDto
    {
        public int Id { get; set; }

        public string Brand { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        public string RegistrationNumber { get; set; }
            = string.Empty;

        public decimal PricePerDay { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public int? CarCategoryId { get; set; }

        public string? CarCategoryName { get; set; }

        public int? BranchId { get; set; }

        public string? BranchName { get; set; }
    }
}
