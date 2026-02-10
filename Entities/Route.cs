namespace ProyectoFinalTecWeb.Entities
{
    public class Route
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Trip> trips { get; set; } = new List<Trip>();
    }
}
