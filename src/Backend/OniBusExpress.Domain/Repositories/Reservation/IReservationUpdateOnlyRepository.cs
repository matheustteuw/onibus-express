namespace OniBusExpress.Domain.Repositories.Reservation
{
    public interface IReservationUpdateOnlyRepository
    {
        Task<Entities.Reservation?> GetByCode(string reservationCode);
        void Update(Entities.Reservation reservation);
    }
}
