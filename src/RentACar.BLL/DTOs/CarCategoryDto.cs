namespace RentACar.BLL.DTOs
{
    public class CarCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public int NumberOfCars { get; set; }
    }
}
