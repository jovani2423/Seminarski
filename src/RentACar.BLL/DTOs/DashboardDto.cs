namespace RentACar.BLL.DTOs
{
    public class DashboardDto
    {
        public int TotalUsers { get; set; }

        public int TotalCars { get; set; }

        public int ActiveCars { get; set; }

        public int TotalReservations { get; set; }

        public int ConfirmedReservations { get; set; }

        public int CancelledReservations { get; set; }

        public decimal TotalRevenue { get; set; }

        public Dictionary<string, int> MonthlyReservationCounts { get; set; } = new();
    }
}
