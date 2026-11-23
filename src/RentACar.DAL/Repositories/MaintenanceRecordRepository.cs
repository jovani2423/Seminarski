using Microsoft.EntityFrameworkCore;
using RentACar.DAL.Entities;
using RentACar.DAL.Identity;

namespace RentACar.DAL.Repositories
{
    public class MaintenanceRecordRepository
    {
        private readonly ApplicationIdentityDbContext _context;

        public MaintenanceRecordRepository(
            ApplicationIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<List<MaintenanceRecord>> GetAllAsync()
        {
            return await _context.MaintenanceRecords
                .Include(m => m.Car)
                .OrderByDescending(m => m.MaintenanceDate)
                .ToListAsync();
        }

        public async Task<List<MaintenanceRecord>> GetByCarIdAsync(
            int carId)
        {
            return await _context.MaintenanceRecords
                .Include(m => m.Car)
                .Where(m => m.CarId == carId)
                .OrderByDescending(m => m.MaintenanceDate)
                .ToListAsync();
        }

        public async Task<MaintenanceRecord?> GetByIdAsync(int id)
        {
            return await _context.MaintenanceRecords
                .Include(m => m.Car)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task<bool> CarExistsAsync(int carId)
        {
            return await _context.Cars
                .AnyAsync(c => c.Id == carId);
        }

        public async Task AddAsync(MaintenanceRecord record)
        {
            await _context.MaintenanceRecords.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MaintenanceRecord record)
        {
            _context.MaintenanceRecords.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var record = await GetByIdAsync(id);

            if (record == null)
                return false;

            _context.MaintenanceRecords.Remove(record);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
