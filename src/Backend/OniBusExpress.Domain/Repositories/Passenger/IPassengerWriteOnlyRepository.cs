namespace OniBusExpress.Domain.Repositories.Passenger
{
    public interface IPassengerWriteOnlyRepository
    {
        Task Add(Entities.Passenger passenger);
    }
}
