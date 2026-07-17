using AutoMapper;
using OniBusExpress.Communication.Responses;
using OniBusExpress.Domain.Repositories.Reservation;
using OniBusExpress.Exceptions;
using OniBusExpress.Exceptions.ExceptionsBase;

namespace OniBusExpress.Application.UseCases.Reservation.GetByCode
{
    public class GetReservationByCodeUseCase : IGetReservationByCodeUseCase
    {
        private readonly IReservationReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetReservationByCodeUseCase(IReservationReadOnlyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseReservationJson> Execute(string reservationCode)
        {
            var reservation = await _repository.GetByCode(reservationCode);

            if (reservation is null)
                throw new NotFoundException(ResourceMessagesException.RESERVATION_NOT_FOUND);

            return _mapper.Map<ResponseReservationJson>(reservation);
        }
    }
}
