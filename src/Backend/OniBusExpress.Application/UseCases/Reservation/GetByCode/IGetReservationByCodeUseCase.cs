using OniBusExpress.Communication.Responses;

namespace OniBusExpress.Application.UseCases.Reservation.GetByCode
{
    public interface IGetReservationByCodeUseCase
    {
        Task<ResponseReservationJson> Execute(string reservationCode);
    }
}
