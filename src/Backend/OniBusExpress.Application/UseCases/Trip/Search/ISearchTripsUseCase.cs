using OniBusExpress.Communication.Responses;

namespace OniBusExpress.Application.UseCases.Trip.Search
{
    public interface ISearchTripsUseCase
    {
        Task<ResponseTripsJson> Execute(string origin, string destination, DateOnly departureDate);
    }
}
