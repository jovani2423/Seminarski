using Microsoft.EntityFrameworkCore;
using RentACar.DAL.Entities;
using RentACar.DAL.Identity;

namespace RentACar.DAL.Repositories
{
    public class BranchRepository
    {
        private readonly ApplicationIdentityDbContext _context;

        public BranchRepository(ApplicationIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<List<Branch>> GetAllAsync()
        {
            return await _context.Branches
                .Include(b => b.Cars)
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<List<Branch>> GetActiveAsync()
        {
            return await _context.Branches
                .Where(b => b.IsActive)
                .OrderBy(b => b.Name)
                .ToListAsync();
        }

        public async Task<Branch?> GetByIdAsync(int id)
        {
            return await _context.Branches
                .Include(b => b.Cars)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            return await _context.Branches
                .AnyAsync(b => b.Id == id && b.IsActive);
        }

        public async Task<bool> NameExistsAsync(
        string name, int? excludeId = null)
        {
            string normalizedName = name.Trim();

            return await _context.Branches.AnyAsync(b =>
                b.Name.Trim().ToLower() == normalizedName.ToLower() &&
                (!excludeId.HasValue || b.Id != excludeId.Value));
        }

        public async Task<bool> IsUsedAsync(int id)
        {
            return await _context.Cars
                .AnyAsync(c => c.BranchId == id);
        }

        public async Task AddAsync(Branch branch)
        {
            await _context.Branches.AddAsync(branch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Branch branch)
        {
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var branch = await GetByIdAsync(id);

            if (branch == null)
                return false;

            if (await IsUsedAsync(id))
                return false;

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            var branch = await GetByIdAsync(id);

            if (branch == null)
                return false;

            branch.IsActive = false;

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
