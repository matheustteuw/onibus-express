namespace OniBusExpress.Application.UseCases.Reservation.Cancel
{
    public interface ICancelReservationUseCase
    {
        Task Execute(string reservationCode);
    }
}
