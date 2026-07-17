namespace OniBusExpress.Domain.Repositories.Passenger
{
    public interface IPassengerReadOnlyRepository
    {
        Task<Entities.Passenger?> GetByCpf(string cpf);
    }
}
