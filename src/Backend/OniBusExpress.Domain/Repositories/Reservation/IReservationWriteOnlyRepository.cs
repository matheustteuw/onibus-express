namespace OniBusExpress.Domain.Repositories.Reservation
{
    public interface IReservationWriteOnlyRepository
    {
        Task Add(Entities.Reservation reservation);
    }
}
