namespace RentACar.BLL.DTOs
{
    public class MaintenanceRecordDto
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public string CarBrand { get; set; } = string.Empty;

        public string CarModel { get; set; } = string.Empty;

        public string RegistrationNumber { get; set; } = string.Empty;

        public DateTime MaintenanceDate { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Cost { get; set; }

        public string Status { get; set; } = "Planned";
    }
}
