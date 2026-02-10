namespace ProyectoFinalTecWeb.Entities
{
    public class Route
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; }
        public ICollection<Trip> trips { get; set; } = new List<Trip>();
    }
}
