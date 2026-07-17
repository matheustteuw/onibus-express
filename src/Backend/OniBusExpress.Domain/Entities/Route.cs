namespace OniBusExpress.Domain.Entities
{
    public class Route
    {
        public Guid Id { get; set; }
        public string Origin { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public TimeSpan EstimatedDuration { get; set; }
        public ICollection<Trip> Trips { get; set; } = [];
    }
}
