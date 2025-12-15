using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.TripDto;
using ProyectoFinalTecWeb.Repositories;

namespace ProyectoFinalTecWeb.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _trips;
        private readonly IDriverRepository _drivers;
        private readonly IPassengerRepository _passengers;

        public TripService(ITripRepository trips, IDriverRepository conductor, IPassengerRepository pasajero)
        {
            _trips = trips;
            _drivers = conductor;
            _passengers = pasajero;
        }
        public async Task<Guid> CreateAsync(CreateTripDto dto)
        {
            // Cargar Passenger y Driver CON SUS TRIPS
            var passenger = await _passengers.GetByIdWithTripsAsync(dto.PassengerId);
            var driver = await _drivers.GetByIdWithTripsAsync(dto.DriverId);

            if (passenger == null || driver == null)
                throw new Exception("Passenger or Driver not found");

            // Crear trip
            var trip = new Trip
            {
                Id = Guid.NewGuid(),
                Origin = dto.Origin,
                Destiny = dto.Destiny,
                Price = dto.Price,
                StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
                PassengerId = dto.PassengerId,
                DriverId = dto.DriverId,
                Passenger = passenger,
                Driver = driver
            };

            // CORREGIDO: Agregar el trip a las colecciones
            passenger.Trips.Add(trip);
            driver.Trips.Add(trip);

            // Guardar los cambios (esto actualizará todas las entidades)
            await _trips.AddAsync(trip);
            await _trips.SaveChangesAsync();

            return trip.Id;
        }

        public async Task<IEnumerable<TripDto>> GetAllAsync()
        {
            var trips = await _trips.GetAllAsync();

            return trips.Select(t => new TripDto
            {
                Id = t.Id,
                Origin = t.Origin,
                Destiny = t.Destiny,
                Price = t.Price,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Passenger = new PassengerInfoDto
                {
                    Id = t.Passenger.Id,
                    Name = t.Passenger.Name,
                    Email = t.Passenger.Email,
                    Phone = t.Passenger.Phone
                },
                Driver = new DriverInfoDto
                {
                    Id = t.Driver.Id,
                    Name = t.Driver.Name,
                    Email = t.Driver.Email,
                    Licence = t.Driver.Licence
                }
            });
        }

        public async Task<IEnumerable<Trip>> GetAllAsyncNormal()
        {
            return await _trips.GetAllAsync();
        }

        public async Task<TripDto?> GetByIdAsync(Guid id)
        {
            var trip = await _trips.GetTripAsync(id);
            if (trip == null) return null;

            return new TripDto
            {
                Id = trip.Id,
                Origin = trip.Origin,
                Destiny = trip.Destiny,
                Price = trip.Price,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                Passenger = new PassengerInfoDto
                {
                    Id = trip.Passenger.Id,
                    Name = trip.Passenger.Name,
                    Email = trip.Passenger.Email,
                    Phone = trip.Passenger.Phone
                },
                Driver = new DriverInfoDto
                {
                    Id = trip.Driver.Id,
                    Name = trip.Driver.Name,
                    Email = trip.Driver.Email,
                    Licence = trip.Driver.Licence
                }
            };
        }

        public async Task<Trip?> GetByIdAsyncNormal(Guid id)
        {
            return await _trips.GetTripAsync(id);
        }
    }
}
