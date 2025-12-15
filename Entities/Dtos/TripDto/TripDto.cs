namespace ProyectoFinalTecWeb.Entities.Dtos.TripDto
{
    public class TripDto
    {
        public Guid Id { get; set; }
        public string Origin { get; set; }
        public string Destiny { get; set; }
        public int Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Información básica del Passenger
        public PassengerInfoDto Passenger { get; set; }

        // Información básica del Driver
        public DriverInfoDto Driver { get; set; }
    }

    public class PassengerInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        // NO incluir Trips aquí
    }

    public class DriverInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Licence { get; set; }
        // NO incluir Trips ni Vehicles aquí
    }
}
