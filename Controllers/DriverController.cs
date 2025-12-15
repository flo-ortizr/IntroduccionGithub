using Microsoft.AspNetCore.Mvc;
using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.DriverDto;
using ProyectoFinalTecWeb.Services;

namespace ProyectoFinalTecWeb.Controllers
{
        [ApiController]
        [Route("api/driver")]
        public class DriverController : ControllerBase
        {
            private readonly IDriverService _service;
            private readonly ITripService _trips;
        private readonly IDriverVehicleService _driverVehicleService;
            public DriverController(IDriverService service, ITripService trips, IDriverVehicleService driverVehicle)
            {
                _service = service;
                _trips = trips;
                _driverVehicleService = driverVehicle;
            }

            // GET: api/driver
            [HttpGet]
            public async Task<IActionResult> GetAllDriveres()
            {
                IEnumerable<DriverDto> items = await _service.GetAll();
                return Ok(items);
            }

            // GET: api/driver/{id}
            [HttpGet("{id:guid}")]
            public async Task<IActionResult> GetOne(Guid id)
            {
                var driver = await _service.GetOne(id);
                if (driver == null) return NotFound();
                return Ok(driver);
            }

            

            // POST: api/driver
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] CreateDriverDto dto)
            {
                var id = await _service.CreateAsync(dto);
                return Created($"api/driver/{id}", new { id });
            }

            // PUT: api/driver/{id}
            [HttpPut("{id:guid}")]
            public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverDto dto, Guid id)
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);
                var driver = await _service.UpdateDriver(dto, id);
                return CreatedAtAction(nameof(GetOne), new { id = driver.Id }, driver);
            }

            // DELETE: api/driver/{id}
            [HttpDelete("{id:guid}")]
            public async Task<IActionResult> DeleteDriver(Guid id)
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);
                await _service.DeleteDriver(id);
                return NoContent();
            }

            // GET: api/driver/{driverId}/vehicles
            [HttpGet("{driverId:guid}/vehicles")]
            public async Task<IActionResult> GetVehiclesByDriver(Guid driverId)
            {
                var vehicles = await _driverVehicleService.GetVehiclesByDriver(driverId);
                return Ok(vehicles);
            }

    }
}
