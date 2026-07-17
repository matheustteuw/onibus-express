using AutoMapper;
using OniBusExpress.Communication.Responses;
using OniBusExpress.Domain.Repositories.Trip;
using OniBusExpress.Exceptions;
using OniBusExpress.Exceptions.ExceptionsBase;

namespace OniBusExpress.Application.UseCases.Trip.GetById
{
    public class GetTripByIdUseCase : IGetTripByIdUseCase
    {
        private readonly ITripReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetTripByIdUseCase(ITripReadOnlyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseTripDetailsJson> Execute(Guid tripId)
        {
            var trip = await _repository.GetById(tripId);

            if (trip is null)
                throw new NotFoundException(ResourceMessagesException.TRIP_NOT_FOUND);

            return _mapper.Map<ResponseTripDetailsJson>(trip);
        }
    }
}
