using AutoMapper;
using OniBusExpress.Communication.Responses;
using OniBusExpress.Domain.Repositories.Trip;

namespace OniBusExpress.Application.UseCases.Trip.Search
{
    public class SearchTripsUseCase : ISearchTripsUseCase
    {
        private readonly ITripReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public SearchTripsUseCase(ITripReadOnlyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseTripsJson> Execute(string origin, string destination, DateOnly departureDate)
        {
            var trips = await _repository.Search(origin, destination, departureDate);

            return new ResponseTripsJson
            {
                Trips = _mapper.Map<IList<ResponseShortTripJson>>(trips)
            };
        }
    }
}
