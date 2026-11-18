using Microsoft.EntityFrameworkCore;
using RentACar.DAL.Identity;

namespace RentACar.DAL.Repositories
{
    public class UserRepository
    {
        private readonly ApplicationIdentityDbContext _context;

        public UserRepository(ApplicationIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Users.CountAsync();
        }
    }
}
