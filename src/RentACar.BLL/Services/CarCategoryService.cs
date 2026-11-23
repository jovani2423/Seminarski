using RentACar.BLL.DTOs;
using RentACar.DAL.Entities;
using RentACar.DAL.Repositories;

namespace RentACar.BLL.Services
{
    public class CarCategoryService
    {
        private readonly CarCategoryRepository _repository;

        public CarCategoryService(
            CarCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CarCategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();

            return categories.Select(c => new CarCategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                NumberOfCars = c.Cars.Count
            }).ToList();
        }

        public async Task<CarCategoryDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var category = await _repository.GetByIdAsync(id);

            if (category == null)
                return null;

            return new CarCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                NumberOfCars = category.Cars.Count
            };
        }

        public async Task<bool> AddAsync(CarCategoryDto dto)
        {
            if (!IsValid(dto))
                return false;

            string name = dto.Name.Trim();

            if (await _repository.NameExistsAsync(name))
                return false;

            var category = new CarCategory
            {
                Name = name,
                Description = dto.Description?.Trim(),
                IsActive = dto.IsActive
            };

            await _repository.AddAsync(category);

            return true;
        }

        public async Task<bool> UpdateAsync(CarCategoryDto dto)
        {
            if (dto.Id <= 0 || !IsValid(dto))
                return false;

            string name = dto.Name.Trim();

            var category = await _repository.GetByIdAsync(dto.Id);

            if (category == null)
                return false;

            if (await _repository.NameExistsAsync(name, dto.Id))
                return false;

            category.Name = name;
            category.Description = dto.Description?.Trim();
            category.IsActive = dto.IsActive;

            await _repository.UpdateAsync(category);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _repository.DeleteAsync(id);
        }

        private static bool IsValid(CarCategoryDto dto)
        {
            return !string.IsNullOrWhiteSpace(dto.Name) &&
                   dto.Name.Trim().Length <= 50 &&
                   (dto.Description?.Length ?? 0) <= 300;
        }
    }
}
