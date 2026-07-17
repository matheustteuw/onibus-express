using OniBusExpress.Communication.Responses;

namespace OniBusExpress.Application.UseCases.Reservation.GetById
{
    public interface IGetReservationByIdUseCase
    {
        Task<ResponseReservationJson> Execute(string reservationCode);
    }
}
