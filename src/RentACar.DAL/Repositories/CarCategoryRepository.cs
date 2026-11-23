using Microsoft.EntityFrameworkCore;
using RentACar.DAL.Entities;
using RentACar.DAL.Identity;

namespace RentACar.DAL.Repositories
{
    public class CarCategoryRepository
    {
        private readonly ApplicationIdentityDbContext _context;

        public CarCategoryRepository(
            ApplicationIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarCategory>> GetAllAsync()
        {
            return await _context.CarCategories
                .Include(c => c.Cars)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<CarCategory?> GetByIdAsync(int id)
        {
            return await _context.CarCategories
                .Include(c => c.Cars)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> NameExistsAsync(
        string name, int? excludeId = null)
        {
            string normalizedName = name.Trim();

            return await _context.CarCategories.AnyAsync(c =>
                c.Name.Trim().ToLower() == normalizedName.ToLower() &&
                (!excludeId.HasValue || c.Id != excludeId.Value));
        }

        public async Task<bool> IsUsedAsync(int id)
        {
            return await _context.Cars
                .AnyAsync(c => c.CarCategoryId == id);
        }

        public async Task AddAsync(CarCategory category)
        {
            await _context.CarCategories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CarCategory category)
        {
            _context.CarCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await GetByIdAsync(id);

            if (category == null)
                return false;

            if (await IsUsedAsync(id))
                return false;

            _context.CarCategories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
