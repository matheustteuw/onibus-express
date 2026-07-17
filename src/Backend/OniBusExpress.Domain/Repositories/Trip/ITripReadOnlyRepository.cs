namespace OniBusExpress.Domain.Repositories.Trip
{
    public interface ITripReadOnlyRepository
    {
        Task<IList<Entities.Trip>> Search(string origin, string destination, DateOnly departureDate);
        Task<Entities.Trip?> GetById(Guid tripId);
    }
}
