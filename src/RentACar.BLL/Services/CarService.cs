using RentACar.BLL.DTOs;
using RentACar.DAL.Entities;
using RentACar.DAL.Repositories;

namespace RentACar.BLL.Services
{
    public class CarService
    {
        private readonly CarRepository _carRepository;
        private readonly CarCategoryRepository _categoryRepository;
        private readonly BranchRepository _branchRepository;

        public CarService(
            CarRepository carRepository,
            CarCategoryRepository categoryRepository,
            BranchRepository branchRepository)
        {
            _carRepository = carRepository;
            _categoryRepository = categoryRepository;
            _branchRepository = branchRepository;
        }

        public async Task<List<CarDto>> GetAllCarsAsync()
        {
            var cars = await _carRepository.GetAllActiveAsync();

            return cars.Select(car => new CarDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                RegistrationNumber = car.RegistrationNumber,
                PricePerDay = car.PricePerDay,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                CarCategoryId = car.CarCategoryId,
                CarCategoryName = car.CarCategory?.Name,
                BranchId = car.BranchId,
                BranchName = car.Branch?.Name
            }).ToList();
        }

        public async Task<CarDto?> GetCarByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var car = await _carRepository.GetByIdAsync(id);

            if (car == null)
                return null;

            return new CarDto
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                RegistrationNumber = car.RegistrationNumber,
                PricePerDay = car.PricePerDay,
                Description = car.Description,
                ImageUrl = car.ImageUrl,
                CarCategoryId = car.CarCategoryId,
                CarCategoryName = car.CarCategory?.Name,
                BranchId = car.BranchId,
                BranchName = car.Branch?.Name
            };
        }

        public async Task<bool> AddCarAsync(CarDto carDto)
        {
            if (!IsValidCar(carDto))
                return false;

            if (!await AreAssignmentsValidAsync(
                carDto.CarCategoryId, carDto.BranchId))
                return false;

            string registration =
                carDto.RegistrationNumber.Trim().ToUpperInvariant();

            bool registrationExists =
                await _carRepository.RegistrationNumberExistsAsync(
                    registration);

            if (registrationExists)
                return false;

            var car = new Car
            {
                Brand = carDto.Brand.Trim(),
                Model = carDto.Model.Trim(),
                Year = carDto.Year,
                RegistrationNumber = registration,
                PricePerDay = carDto.PricePerDay,
                Description = carDto.Description?.Trim(),
                ImageUrl = carDto.ImageUrl?.Trim(),
                CarCategoryId = carDto.CarCategoryId,
                BranchId = carDto.BranchId,
                IsActive = true
            };

            await _carRepository.AddAsync(car);

            return true;
        }

        public async Task<bool> UpdateCarAsync(CarDto carDto)
        {
            if (carDto.Id <= 0 || !IsValidCar(carDto))
                return false;

            var existingCar =
                await _carRepository.GetByIdAsync(carDto.Id);

            if (existingCar == null)
                return false;

            if (!await AreAssignmentsValidAsync(
                carDto.CarCategoryId, carDto.BranchId))
                return false;

            string registration =
                carDto.RegistrationNumber.Trim().ToUpperInvariant();

            bool registrationExists =
                await _carRepository.RegistrationNumberExistsAsync(
                    registration, carDto.Id);

            if (registrationExists)
                return false;

            existingCar.Brand = carDto.Brand.Trim();
            existingCar.Model = carDto.Model.Trim();
            existingCar.Year = carDto.Year;
            existingCar.RegistrationNumber = registration;
            existingCar.PricePerDay = carDto.PricePerDay;
            existingCar.Description = carDto.Description?.Trim();
            existingCar.ImageUrl = carDto.ImageUrl?.Trim();
            existingCar.CarCategoryId = carDto.CarCategoryId;
            existingCar.BranchId = carDto.BranchId;

            await _carRepository.UpdateAsync(existingCar);

            return true;
        }

        private static bool IsValidCar(CarDto dto)
        {
            return !string.IsNullOrWhiteSpace(dto.Brand) &&
                   !string.IsNullOrWhiteSpace(dto.Model) &&
                   !string.IsNullOrWhiteSpace(dto.RegistrationNumber) &&
                   dto.Brand.Trim().Length <= 50 &&
                   dto.Model.Trim().Length <= 50 &&
                   dto.RegistrationNumber.Trim().Length <= 20 &&
                   dto.Year >= 1900 &&
                   dto.Year <= 2100 &&
                   dto.PricePerDay >= 0.01m &&
                   dto.PricePerDay <= 99999999.99m &&
                   (dto.Description?.Trim().Length ?? 0) <= 1000 &&
                   (dto.ImageUrl?.Trim().Length ?? 0) <= 500;
        }

        private async Task<bool> AreAssignmentsValidAsync(
            int? categoryId, int? branchId)
        {
            if (categoryId.HasValue)
            {
                var category =
                    await _categoryRepository.GetByIdAsync(
                        categoryId.Value);

                if (category == null || !category.IsActive)
                    return false;
            }

            if (branchId.HasValue)
            {
                bool branchIsActive =
                    await _branchRepository.IsActiveAsync(
                        branchId.Value);

                if (!branchIsActive)
                    return false;
            }

            return true;
        }

        public async Task<bool> DeactivateCarAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _carRepository.DeactivateAsync(id);
        }

        public async Task<List<CarDto>> SearchCarsAsync(
            CarSearchDto searchDto)
        {
            if (searchDto == null)
                return new List<CarDto>();

            if (searchDto.CarCategoryId.HasValue &&
                searchDto.CarCategoryId.Value <= 0)
                return new List<CarDto>();

            if (searchDto.BranchId.HasValue &&
                searchDto.BranchId.Value <= 0)
                return new List<CarDto>();

            if (searchDto.MinPrice.HasValue &&
                searchDto.MinPrice.Value < 0)
                return new List<CarDto>();

            if (searchDto.MaxPrice.HasValue &&
                searchDto.MaxPrice.Value < 0)
                return new List<CarDto>();

            if (searchDto.MinPrice.HasValue &&
                searchDto.MaxPrice.HasValue &&
                searchDto.MinPrice.Value > searchDto.MaxPrice.Value)
                return new List<CarDto>();

            if (searchDto.MinYear.HasValue &&
                searchDto.MaxYear.HasValue &&
                searchDto.MinYear.Value > searchDto.MaxYear.Value)
                return new List<CarDto>();

            if (searchDto.MinYear.HasValue &&
                (searchDto.MinYear.Value < 1900 ||
                 searchDto.MinYear.Value > 2100))
                return new List<CarDto>();

            if (searchDto.MaxYear.HasValue &&
                (searchDto.MaxYear.Value < 1900 ||
                 searchDto.MaxYear.Value > 2100))
                return new List<CarDto>();

            var cars = await _carRepository.SearchAsync(
                searchDto.SearchTerm,
                searchDto.MinPrice,
                searchDto.MaxPrice,
                searchDto.MinYear,
                searchDto.MaxYear,
                searchDto.CarCategoryId,
                searchDto.BranchId);

            return cars.Select(c => new CarDto
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                RegistrationNumber = c.RegistrationNumber,
                PricePerDay = c.PricePerDay,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                CarCategoryId = c.CarCategoryId,
                CarCategoryName = c.CarCategory?.Name,
                BranchId = c.BranchId,
                BranchName = c.Branch?.Name
            }).ToList();
        }
    }
}
