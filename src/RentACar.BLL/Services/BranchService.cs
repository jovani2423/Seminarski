using RentACar.BLL.DTOs;
using RentACar.DAL.Entities;
using RentACar.DAL.Repositories;

namespace RentACar.BLL.Services
{
    public class BranchService
    {
        private readonly BranchRepository _repository;

        public BranchService(BranchRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<BranchDto>> GetAllAsync()
        {
            var branches = await _repository.GetAllAsync();

            return branches.Select(b => new BranchDto
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                City = b.City,
                PhoneNumber = b.PhoneNumber,
                IsActive = b.IsActive,
                NumberOfCars = b.Cars.Count
            }).ToList();
        }

        public async Task<List<Branch>> GetActiveAsync()
        {
            return await _repository.GetActiveAsync();
        }

        public async Task<BranchDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var branch = await _repository.GetByIdAsync(id);

            if (branch == null)
                return null;

            return new BranchDto
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                City = branch.City,
                PhoneNumber = branch.PhoneNumber,
                IsActive = branch.IsActive,
                NumberOfCars = branch.Cars.Count
            };
        }

        public async Task<bool> IsActiveAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _repository.IsActiveAsync(id);
        }

        public async Task<bool> AddAsync(BranchDto dto)
        {
            if (!IsValid(dto))
                return false;

            string name = dto.Name.Trim();

            if (await _repository.NameExistsAsync(name))
                return false;

            var branch = new Branch
            {
                Name = name,
                Address = dto.Address.Trim(),
                City = dto.City?.Trim(),
                PhoneNumber = dto.PhoneNumber?.Trim(),
                IsActive = dto.IsActive
            };

            await _repository.AddAsync(branch);

            return true;
        }

        public async Task<bool> UpdateAsync(BranchDto dto)
        {
            if (dto.Id <= 0 || !IsValid(dto))
                return false;

            string name = dto.Name.Trim();

            var branch = await _repository.GetByIdAsync(dto.Id);

            if (branch == null)
                return false;

            if (await _repository.NameExistsAsync(name, dto.Id))
                return false;

            branch.Name = name;
            branch.Address = dto.Address.Trim();
            branch.City = dto.City?.Trim();
            branch.PhoneNumber = dto.PhoneNumber?.Trim();
            branch.IsActive = dto.IsActive;

            await _repository.UpdateAsync(branch);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> DeactivateAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _repository.DeactivateAsync(id);
        }

        private static bool IsValid(BranchDto dto)
        {
            return !string.IsNullOrWhiteSpace(dto.Name) &&
                   !string.IsNullOrWhiteSpace(dto.Address) &&
                   dto.Name.Trim().Length <= 100 &&
                   dto.Address.Trim().Length <= 200 &&
                   (dto.City?.Trim().Length ?? 0) <= 50 &&
                   (dto.PhoneNumber?.Trim().Length ?? 0) <= 20;
        }
    }
}
