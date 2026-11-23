namespace RentACar.BLL.DTOs
{
    public class BranchDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string? City { get; set; }

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public int NumberOfCars { get; set; }
    }
}
