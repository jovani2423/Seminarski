namespace RentACar.BLL.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public string CarBrand { get; set; } = string.Empty;

        public string CarModel { get; set; } = string.Empty;

        public string RegistrationNumber { get; set; } = string.Empty;

        public string BranchName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string UserFirstName { get; set; } = string.Empty;

        public string UserLastName { get; set; } = string.Empty;

        public string UserEmail { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int NumberOfDays { get; set; }

        public decimal PricePerDay { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
