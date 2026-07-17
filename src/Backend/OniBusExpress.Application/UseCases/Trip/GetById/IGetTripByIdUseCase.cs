using OniBusExpress.Communication.Responses;

namespace OniBusExpress.Application.UseCases.Trip.GetById
{
    public interface IGetTripByIdUseCase
    {
        Task<ResponseTripDetailsJson> Execute(Guid tripId);
    }
}
