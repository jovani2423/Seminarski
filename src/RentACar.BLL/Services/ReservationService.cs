using RentACar.BLL.DTOs;
using RentACar.DAL.Entities;
using RentACar.DAL.Repositories;

namespace RentACar.BLL.Services
{
    public class ReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly CarRepository _carRepository;

        public ReservationService(
            ReservationRepository reservationRepository,
            CarRepository carRepository)
        {
            _reservationRepository = reservationRepository;
            _carRepository = carRepository;
        }

        public async Task<(bool Success, string Message)> CreateReservationAsync(
            ReservationDto dto)
        {
            if (dto.CarId <= 0)
                return (false, "Izaberite ispravan automobil.");

            if (string.IsNullOrWhiteSpace(dto.UserId))
                return (false, "Korisnik nije prijavljen.");

            if (dto.StartDate.Date < DateTime.Today)
                return (false, "Datum preuzimanja ne može biti u prošlosti.");

            if (dto.EndDate.Date <= dto.StartDate.Date)
                return (false, "Datum vraćanja mora biti nakon datuma preuzimanja.");

            var car = await _carRepository.GetByIdAsync(dto.CarId);

            if (car == null)
                return (false, "Izabrani automobil ne postoji ili nije dostupan.");

            bool isReserved =
                await _reservationRepository.HasOverlappingReservationAsync(
                    dto.CarId,
                    dto.StartDate.Date,
                    dto.EndDate.Date);

            if (isReserved)
                return (false, "Automobil je već rezervisan u izabranom periodu.");

            int numberOfDays =
                (dto.EndDate.Date - dto.StartDate.Date).Days;

            decimal totalPrice = numberOfDays * car.PricePerDay;

            var reservation = new Reservation
            {
                CarId = car.Id,
                UserId = dto.UserId,
                StartDate = dto.StartDate.Date,
                EndDate = dto.EndDate.Date,
                TotalPrice = totalPrice,
                Status = "Confirmed",
                CreatedAt = DateTime.Now
            };

            await _reservationRepository.AddAsync(reservation);

            return (true, "Rezervacija je uspešno kreirana.");
        }

        public async Task<List<ReservationDto>> GetUserReservationsAsync(
            string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new List<ReservationDto>();

            var reservations =
                await _reservationRepository.GetByUserIdAsync(userId);

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                CarId = r.CarId,
                CarBrand = r.Car.Brand,
                CarModel = r.Car.Model,
                UserId = r.UserId,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                NumberOfDays = (r.EndDate - r.StartDate).Days,
                PricePerDay = r.Car.PricePerDay,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<(bool Success, string Message)> CancelReservationAsync(
        int reservationId,
        string userId)
        {
            if (reservationId <= 0)
                return (false, "Neispravan broj rezervacije.");

            if (string.IsNullOrWhiteSpace(userId))
                return (false, "Korisnik nije prijavljen.");

            var reservation = await _reservationRepository.GetByIdAsync(reservationId);

            if (reservation == null || reservation.UserId != userId)
                return (false, "Rezervacija nije pronađena.");

            if (reservation.Status == "Cancelled")
                return (false, "Rezervacija je već otkazana.");

            bool cancelled = await _reservationRepository.CancelAsync(
                reservationId,
                userId);

            if (!cancelled)
                return (false, "Rezervaciju nije moguće otkazati.");

            return (true, "Rezervacija je uspešno otkazana.");
        }

        public async Task<List<ReservationDto>> GetAllReservationsAsync()
        {
            var reservations = await _reservationRepository.GetAllAsync();

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                CarId = r.CarId,
                CarBrand = r.Car.Brand,
                CarModel = r.Car.Model,
                UserId = r.UserId,
                UserFirstName = r.User.FirstName ?? string.Empty,
                UserLastName = r.User.LastName ?? string.Empty,
                UserEmail = r.User.Email ?? string.Empty,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                NumberOfDays = (r.EndDate - r.StartDate).Days,
                PricePerDay = r.Car.PricePerDay,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToList();
        }
    }
}
