
using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.DriverDto;
using ProyectoFinalTecWeb.Entities.Dtos.DriverDto;
using ProyectoFinalTecWeb.Entities.Dtos.PassengerDto;
using ProyectoFinalTecWeb.Repositories;
using System.ComponentModel.DataAnnotations;

namespace ProyectoFinalTecWeb.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _drivers;
        private readonly IConfiguration _configuration;

        public DriverService(IDriverRepository drivers, IConfiguration configuration)
        {
            _drivers = drivers;
            _configuration = configuration;
        }

        public async Task DeleteDriver(Guid id)
        {
            Driver? driver = await _drivers.GetOne(id);
            if (driver == null) return;
            await _drivers.Delete(driver);
        }

        public async Task<IEnumerable<DriverDto>> GetAll()
        {
            var drivers = await _drivers.GetAll();

            return drivers.Select(d => new DriverDto
            {
                Id = d.Id,
                Name = d.Name,
                Email = d.Email,
                Licence = d.Licence,
                Phone = d.Phone,
                PasswordHash = d.PasswordHash,
                Role = d.Role,
                // Mapear Trips
                Trips = d.Trips?.Select(t => new TripSimpleDto
                {
                    Id = t.Id,
                    Origin = t.Origin,
                    Destiny = t.Destiny,
                    Price = t.Price,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                }).ToList() ?? new List<TripSimpleDto>(),
                // Mapear Vehicles
                Vehicles = d.Vehicles?.Select(v => new VehicleSimpleDto
                {
                    Id = v.Id,
                    Plate = v.Plate,
                    ModelBrand = v.Model?.Brand ?? "Sin marca",
                    ModelYear = v.Model?.Year ?? 0
                }).ToList() ?? new List<VehicleSimpleDto>()
            });
        }

        public async Task<DriverDto> GetOne(Guid id)
        {
            var driver = await _drivers.GetOne(id);
            if (driver == null) return null;

            return new DriverDto
            {
                Id = driver.Id,
                Name = driver.Name,
                Email = driver.Email,
                Licence = driver.Licence,
                Phone = driver.Phone,
                PasswordHash = driver.PasswordHash,
                Role = driver.Role,
                // Mapear Trips
                Trips = driver.Trips?.Select(t => new TripSimpleDto
                {
                    Id = t.Id,
                    Origin = t.Origin,
                    Destiny = t.Destiny,
                    Price = t.Price,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                }).ToList() ?? new List<TripSimpleDto>(),
                // Mapear Vehicles
                Vehicles = driver.Vehicles?.Select(v => new VehicleSimpleDto
                {
                    Id = v.Id,
                    Plate = v.Plate,
                    ModelBrand = v.Model?.Brand ?? "Sin marca",
                    ModelYear = v.Model?.Year ?? 0
                }).ToList() ?? new List<VehicleSimpleDto>()
            };
        }

        public async Task<IEnumerable<Driver>> GetAllNormal()
        {
            return await _drivers.GetAll();
        }

        public async Task<Driver?> GetOneNormal(Guid id)
        {
            return await _drivers.GetOne(id);
        }

        
        public async Task<string> RegisterAsync(RegisterDriverDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var driver = new Driver
            {
                Email = dto.Email,
                Name = dto.Name
            };
            await _drivers.AddAsync(driver);
            return driver.Id.ToString();
        }
        
        public async Task<Driver> UpdateDriver(UpdateDriverDto dto, Guid id)
        {
            Driver? driver = await GetOneNormal(id);
            if (driver == null) throw new Exception("Driver doesnt exist.");

            driver.Name = dto.Name;
            driver.Licence = dto.Licence;
            driver.Phone = dto.Phone;
            driver.Email = dto.Email;

            await _drivers.Update(driver);
            return driver;
        }

        // Método para obtener Driver con toda la información (incluyendo relaciones)
        public async Task<DriverDto> GetOneWithDetails(Guid id)
        {
            var driver = await _drivers.GetTripsAsync(id); // Este método ya incluye Trips y Vehicles
            if (driver == null) return null;

            return new DriverDto
            {
                Id = driver.Id,
                Name = driver.Name,
                Email = driver.Email,
                Licence = driver.Licence,
                Phone = driver.Phone,
                Role = driver.Role,
                // Mapear Trips
                Trips = driver.Trips?.Select(t => new TripSimpleDto
                {
                    Id = t.Id,
                    Origin = t.Origin,
                    Destiny = t.Destiny,
                    Price = t.Price,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                }).ToList() ?? new List<TripSimpleDto>(),
                // Mapear Vehicles
                Vehicles = driver.Vehicles?.Select(v => new VehicleSimpleDto
                {
                    Id = v.Id,
                    Plate = v.Plate,
                    ModelBrand = v.Model?.Brand ?? "Sin marca",
                    ModelYear = v.Model?.Year ?? 0
                }).ToList() ?? new List<VehicleSimpleDto>()
            };
        }
    }
}
