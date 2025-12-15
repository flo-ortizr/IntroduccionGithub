using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.TripDto;

namespace ProyectoFinalTecWeb.Services
{
    public interface ITripService
    {
        Task<Guid> CreateAsync(CreateTripDto dto);

        Task<Trip?> GetByIdAsyncNormal(Guid id);
        Task<TripDto?> GetByIdAsync(Guid id);

        Task<IEnumerable<Trip>> GetAllAsyncNormal();
        Task<IEnumerable<TripDto>> GetAllAsync();


        //Task<ViajePasajeroDto?> GetPasajeroAsync(int id);
    }
}
