namespace OniBusExpress.Domain.Repositories.Reservation
{
    public interface IReservationReadOnlyRepository
    {
        Task<Entities.Reservation?> GetByCode(string reservationCode);
        Task<bool> ExistsActiveReservationForSeat(Guid tripId, int seatNumber);
        Task<bool> ExistsReservationWithCode(string reservationCode);
    }
}
