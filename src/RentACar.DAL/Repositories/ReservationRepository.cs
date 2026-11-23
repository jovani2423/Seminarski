using Microsoft.EntityFrameworkCore;
using RentACar.DAL.Entities;
using RentACar.DAL.Identity;

namespace RentACar.DAL.Repositories
{
    public class ReservationRepository
    {
        private readonly ApplicationIdentityDbContext _context;

        public ReservationRepository(ApplicationIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasOverlappingReservationAsync(
            int carId,
            DateTime startDate,
            DateTime endDate)
        {
            return await _context.Reservations.AnyAsync(r =>
                r.CarId == carId &&
                (r.Status == "Pending" || r.Status == "Confirmed") &&
                startDate < r.EndDate &&
                endDate > r.StartDate);
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }


        public async Task<List<Reservation>> GetByUserIdAsync(string userId)
        {
            return await _context.Reservations
                .Include(r => r.Car)
                    .ThenInclude(c => c.Branch)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }


        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> CancelAsync(int reservationId, string userId)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r =>
                    r.Id == reservationId &&
                    r.UserId == userId);

            if (reservation == null)
                return false;

            if (reservation.Status != "Pending" &&
                reservation.Status != "Confirmed")
                return false;

            if (reservation.EndDate.Date <= DateTime.Today)
                return false;

            reservation.Status = "Cancelled";

            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<List<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.Car)
                    .ThenInclude(c => c.Branch)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }


        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Reservations.CountAsync();
        }

        public async Task<int> GetConfirmedCountAsync()
        {
            return await _context.Reservations
                .CountAsync(r => r.Status == "Confirmed");
        }

        public async Task<int> GetCancelledCountAsync()
        {
            return await _context.Reservations
                .CountAsync(r => r.Status == "Cancelled");
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Reservations
                .Where(r => r.Status == "Confirmed")
                .SumAsync(r => (decimal?)r.TotalPrice) ?? 0m;
        }


        public async Task<Dictionary<string, int>> GetMonthlyReservationCountsAsync()
        {
            var startDate = new DateTime(
                DateTime.Today.Year,
                DateTime.Today.Month,
                1).AddMonths(-5);

            var monthlyCounts = await _context.Reservations
                .Where(r => r.CreatedAt >= startDate)
                .GroupBy(r => new
                {
                    r.CreatedAt.Year,
                    r.CreatedAt.Month
                })
                .Select(g => new
                {
                    g.Key.Year,
                    g.Key.Month,
                    Count = g.Count()
                })
                .ToListAsync();

            return monthlyCounts.ToDictionary(
                x => $"{x.Year}-{x.Month:D2}",
                x => x.Count);
        }

    }
}
