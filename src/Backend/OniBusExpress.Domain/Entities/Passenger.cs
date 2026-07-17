namespace OniBusExpress.Domain.Entities
{
    public class Passenger
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
