using OniBusExpress.Communication.Requests;
using OniBusExpress.Communication.Responses;

namespace OniBusExpress.Application.UseCases.Reservation.Register
{
    public interface IRegisterReservationUseCase
    {
        Task<ResponseRegisteredReservationJson> Execute(RequestRegisterReservationJson request);
    }
}
