using Microsoft.EntityFrameworkCore;
using RentACar.DAL.Entities;
using RentACar.DAL.Identity;

namespace RentACar.DAL.Repositories
{
    public class CarRepository
    {
        private readonly ApplicationIdentityDbContext _context;

        public CarRepository(ApplicationIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAllActiveAsync()
        {
            return await _context.Cars
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await _context.Cars
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task AddAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RegistrationNumberExistsAsync(
            string registrationNumber,
            int? excludeCarId = null)
        {
            return await _context.Cars.AnyAsync(c =>
                c.RegistrationNumber == registrationNumber &&
                c.Id != excludeCarId);
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var car = await _context.Cars
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (car == null)
                return false;

            car.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Car>> SearchAsync(
        string? searchTerm,
        decimal? minPrice,
        decimal? maxPrice,
        int? minYear,
        int? maxYear)
        {
            var query = _context.Cars
                .Where(c => c.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string term = searchTerm.Trim().ToLower();

                query = query.Where(c =>
                    c.Brand.ToLower().Contains(term) ||
                    c.Model.ToLower().Contains(term));
            }

            if (minPrice.HasValue)
                query = query.Where(c => c.PricePerDay >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(c => c.PricePerDay <= maxPrice.Value);

            if (minYear.HasValue)
                query = query.Where(c => c.Year >= minYear.Value);

            if (maxYear.HasValue)
                query = query.Where(c => c.Year <= maxYear.Value);

            return await query
                .OrderBy(c => c.PricePerDay)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Cars.CountAsync();
        }

        public async Task<int> GetActiveCountAsync()
        {
            return await _context.Cars.CountAsync(c => c.IsActive);
        }
    }
}