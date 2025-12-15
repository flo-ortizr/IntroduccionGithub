using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.PassengerDto;

namespace ProyectoFinalTecWeb.Services
{
    public interface IPassengerService
    {
        //Authentication
        Task<string> RegisterAsync(RegisterPassengerDto dto);

        //CRUD
        Task<Guid> CreateAsync(CreatePassengerDto dto);
        Task<IEnumerable<PassengerDto>> GetAll();
        Task<PassengerDto> GetOne(Guid id);
        Task<IEnumerable<Passenger>> GetAllNormal();
        Task<Passenger> GetOneNormal(Guid id);
        Task<Passenger> UpdatePassenger(UpdatePassengerDto dto, Guid id);
        Task DeletePassenger(Guid id);
    }
}
