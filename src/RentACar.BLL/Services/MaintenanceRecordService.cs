using RentACar.BLL.DTOs;
using RentACar.DAL.Entities;
using RentACar.DAL.Repositories;

namespace RentACar.BLL.Services
{
    public class MaintenanceRecordService
    {
        private readonly MaintenanceRecordRepository _repository;

        private static readonly string[] AllowedStatuses =
        {
            "Planned",
            "InProgress",
            "Completed"
        };

        public MaintenanceRecordService(
            MaintenanceRecordRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MaintenanceRecordDto>> GetAllAsync()
        {
            var records = await _repository.GetAllAsync();

            return records.Select(MapToDto).ToList();
        }

        public async Task<List<MaintenanceRecordDto>> GetByCarIdAsync(
            int carId)
        {
            var records = await _repository.GetByCarIdAsync(carId);

            return records.Select(MapToDto).ToList();
        }

        public async Task<MaintenanceRecordDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
                return null;

            var record = await _repository.GetByIdAsync(id);

            return record == null ? null : MapToDto(record);
        }

        public async Task<bool> AddAsync(MaintenanceRecordDto dto)
        {
            if (!IsValid(dto))
                return false;

            if (!await _repository.CarExistsAsync(dto.CarId))
                return false;

            var record = new MaintenanceRecord
            {
                CarId = dto.CarId,
                MaintenanceDate = dto.MaintenanceDate,
                Description = dto.Description.Trim(),
                Cost = dto.Cost,
                Status = dto.Status
            };

            await _repository.AddAsync(record);

            return true;
        }

        public async Task<bool> UpdateAsync(MaintenanceRecordDto dto)
        {
            if (dto.Id <= 0 || !IsValid(dto))
                return false;

            if (!await _repository.CarExistsAsync(dto.CarId))
                return false;

            var record = await _repository.GetByIdAsync(dto.Id);

            if (record == null)
                return false;

            record.CarId = dto.CarId;
            record.MaintenanceDate = dto.MaintenanceDate;
            record.Description = dto.Description.Trim();
            record.Cost = dto.Cost;
            record.Status = dto.Status;

            await _repository.UpdateAsync(record);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                return false;

            return await _repository.DeleteAsync(id);
        }

        private static bool IsValid(MaintenanceRecordDto dto)
        {
            return dto.CarId > 0 &&
                   !string.IsNullOrWhiteSpace(dto.Description) &&
                   dto.Description.Trim().Length <= 1000 &&
                   dto.MaintenanceDate != default &&
                   dto.MaintenanceDate <= DateTime.Now &&
                   dto.Cost >= 0 &&
                   dto.Cost <= 99999999.99m &&
                   AllowedStatuses.Contains(dto.Status);
        }

        private static MaintenanceRecordDto MapToDto(
            MaintenanceRecord record)
        {
            return new MaintenanceRecordDto
            {
                Id = record.Id,
                CarId = record.CarId,
                CarBrand = record.Car.Brand,
                CarModel = record.Car.Model,
                RegistrationNumber = record.Car.RegistrationNumber,
                MaintenanceDate = record.MaintenanceDate,
                Description = record.Description,
                Cost = record.Cost,
                Status = record.Status
            };
        }
    }
}
