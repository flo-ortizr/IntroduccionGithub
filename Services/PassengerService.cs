using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.PassengerDto;
using ProyectoFinalTecWeb.Repositories;

namespace ProyectoFinalTecWeb.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _pasajeros;
        private readonly IConfiguration _configuration;
        public PassengerService(IPassengerRepository pasajeros, IConfiguration configuration)
        {
            _pasajeros = pasajeros;
            _configuration = configuration;
        }
        public async Task<Guid> CreateAsync(CreatePassengerDto dto)
        {
            var entity = new Passenger { Name = dto.Name, Phone = dto.Phone, Email = dto.Email };
            await _pasajeros.AddAsync(entity);
            await _pasajeros.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeletePassenger(Guid id)
        {
            Passenger? pasajero = await _pasajeros.GetOne(id);
            if (pasajero == null) return;
            await _pasajeros.Delete(pasajero);

        }

        public async Task<IEnumerable<PassengerDto>> GetAll()
        {
            var passengers = await _pasajeros.GetAllWithTripsAsync();

            return passengers.Select(p => new PassengerDto
            {
                Id = p.Id,
                Name = p.Name,
                Phone = p.Phone,
                Email = p.Email,
                Role = p.Role,
                Trips = p.Trips.Select(t => new TripSimpleDto
                {
                    Id = t.Id,
                    Origin = t.Origin,
                    Destiny = t.Destiny,
                    Price = t.Price,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                }).ToList()
            });
        }

        public async Task<PassengerDto> GetOne(Guid id)
        {
            var passenger = await _pasajeros.GetByIdWithTripsAsync(id);
            if (passenger == null) return null;

            return new PassengerDto
            {
                Id = passenger.Id,
                Name = passenger.Name,
                Phone = passenger.Phone,
                Email = passenger.Email,
                Role = passenger.Role,
                Trips = passenger.Trips.Select(t => new TripSimpleDto
                {
                    Id = t.Id,
                    Origin = t.Origin,
                    Destiny = t.Destiny,
                    Price = t.Price,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate
                }).ToList()
            };
        }
        public async Task<IEnumerable<Passenger>> GetAllNormal()
        {
            return await _pasajeros.GetAll();
        }

        public async Task<Passenger> GetOneNormal(Guid id)
        {
            return await _pasajeros.GetOne(id);
        }


        
        public async Task<string> RegisterAsync(RegisterPassengerDto dto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var pasajero = new Passenger
            {
                Email = dto.Email,
                Name = dto.Name
            };
            await _pasajeros.AddAsync(pasajero);
            return pasajero.Id.ToString();
        }
        
        public async Task<Passenger> UpdatePassenger(UpdatePassengerDto dto, Guid id)
        {
            Passenger? pasajero = await GetOneNormal(id);
            if (pasajero == null) throw new Exception("Passenger doesnt exist.");

            pasajero.Name = dto.Name;
            pasajero.Phone = dto.Phone;
            pasajero.Email = dto.Email;

            await _pasajeros.Update(pasajero);
            return pasajero;
        }
    }
}
