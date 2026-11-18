using RentACar.BLL.DTOs;
using RentACar.DAL.Entities;
using RentACar.DAL.Repositories;

namespace RentACar.BLL.Services
{
    public class CarService
    {
        private readonly CarRepository _carRepository;

        public CarService(CarRepository carRepository)
        {
            _carRepository = carRepository;
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
                ImageUrl = car.ImageUrl
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
                ImageUrl = car.ImageUrl
            };
        }

        public async Task<bool> AddCarAsync(CarDto carDto)
        {
            if (string.IsNullOrWhiteSpace(carDto.Brand) ||
                string.IsNullOrWhiteSpace(carDto.Model) ||
                string.IsNullOrWhiteSpace(carDto.RegistrationNumber))
            {
                return false;
            }

            if (carDto.Year < 1900 ||
                carDto.Year > DateTime.Now.Year + 1)
                return false;

            if (carDto.PricePerDay <= 0)
                return false;

            string registration =
                carDto.RegistrationNumber.Trim().ToUpper();

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
                Description = carDto.Description,
                ImageUrl = carDto.ImageUrl,
                IsActive = true
            };

            await _carRepository.AddAsync(car);

            return true;
        }

        public async Task<bool> UpdateCarAsync(CarDto carDto)
        {
            if (carDto.Id <= 0 ||
                string.IsNullOrWhiteSpace(carDto.Brand) ||
                string.IsNullOrWhiteSpace(carDto.Model) ||
                string.IsNullOrWhiteSpace(carDto.RegistrationNumber) ||
                carDto.PricePerDay <= 0 ||
                carDto.Year < 1900 ||
                carDto.Year > DateTime.Now.Year + 1)
            {
                return false;
            }

            var existingCar =
                await _carRepository.GetByIdAsync(carDto.Id);

            if (existingCar == null)
                return false;

            string registration =
                carDto.RegistrationNumber.Trim().ToUpper();

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
            existingCar.Description = carDto.Description;
            existingCar.ImageUrl = carDto.ImageUrl;

            await _carRepository.UpdateAsync(existingCar);

            return true;
        }

        public async Task<bool> DeactivateCarAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _carRepository.DeactivateAsync(id);
        }


        public async Task<List<CarDto>> SearchCarsAsync(CarSearchDto searchDto)
        {
            if (searchDto.MinPrice.HasValue &&
                searchDto.MinPrice.Value < 0)
            {
                return new List<CarDto>();
            }

            if (searchDto.MaxPrice.HasValue &&
                searchDto.MaxPrice.Value < 0)
            {
                return new List<CarDto>();
            }

            if (searchDto.MinPrice.HasValue &&
                searchDto.MaxPrice.HasValue &&
                searchDto.MinPrice.Value > searchDto.MaxPrice.Value)
            {
                return new List<CarDto>();
            }

            if (searchDto.MinYear.HasValue &&
                searchDto.MaxYear.HasValue &&
                searchDto.MinYear.Value > searchDto.MaxYear.Value)
            {
                return new List<CarDto>();
            }

            var cars = await _carRepository.SearchAsync(
                searchDto.SearchTerm,
                searchDto.MinPrice,
                searchDto.MaxPrice,
                searchDto.MinYear,
                searchDto.MaxYear);

            return cars.Select(c => new CarDto
            {
                Id = c.Id,
                Brand = c.Brand,
                Model = c.Model,
                Year = c.Year,
                RegistrationNumber = c.RegistrationNumber,
                PricePerDay = c.PricePerDay,
                Description = c.Description,
                ImageUrl = c.ImageUrl
            }).ToList();
        }

    }
}
