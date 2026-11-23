using RentACar.BLL.DTOs;
using RentACar.DAL.Repositories;

namespace RentACar.BLL.Services
{
    public class DashboardService
    {
        private readonly UserRepository _userRepository;
        private readonly CarRepository _carRepository;
        private readonly ReservationRepository _reservationRepository;

        public DashboardService(
            UserRepository userRepository,
            CarRepository carRepository,
            ReservationRepository reservationRepository)
        {
            _userRepository = userRepository;
            _carRepository = carRepository;
            _reservationRepository = reservationRepository;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var monthlyCounts =
                await _reservationRepository.GetMonthlyReservationCountsAsync();

            var monthlyReservations = new Dictionary<string, int>();

            for (int i = 5; i >= 0; i--)
            {
                var month = DateTime.Today.AddMonths(-i);
                string key = $"{month.Year}-{month.Month:D2}";

                monthlyReservations[key] = monthlyCounts.GetValueOrDefault(key, 0);
            }

            var dashboard = new DashboardDto
            {
                TotalUsers = await _userRepository.GetTotalCountAsync(),
                TotalCars = await _carRepository.GetTotalCountAsync(),
                ActiveCars = await _carRepository.GetActiveCountAsync(),
                TotalReservations = await _reservationRepository.GetTotalCountAsync(),
                ConfirmedReservations = await _reservationRepository.GetConfirmedCountAsync(),
                CancelledReservations = await _reservationRepository.GetCancelledCountAsync(),
                TotalRevenue = await _reservationRepository.GetTotalRevenueAsync(),
                MonthlyReservationCounts = monthlyReservations
            };

            return dashboard;
        }
    }
}
