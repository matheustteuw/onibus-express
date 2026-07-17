using OniBusExpress.Domain.Enums;
using OniBusExpress.Domain.Repositories;
using OniBusExpress.Domain.Repositories.Reservation;
using OniBusExpress.Exceptions;
using OniBusExpress.Exceptions.ExceptionsBase;

namespace OniBusExpress.Application.UseCases.Reservation.Cancel
{
    public class CancelReservationUseCase : ICancelReservationUseCase
    {
        private static readonly TimeSpan CancellationDeadline = TimeSpan.FromHours(2);

        private readonly IReservationUpdateOnlyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelReservationUseCase(IReservationUpdateOnlyRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(string reservationCode)
        {
            var reservation = await _repository.GetByCode(reservationCode);

            if (reservation is null)
                throw new NotFoundException(ResourceMessagesException.RESERVATION_NOT_FOUND);

            if (reservation.Status == ReservationStatus.Cancelled)
                throw new ConflictException(ResourceMessagesException.RESERVATION_ALREADY_CANCELLED);

            var timeUntilDeparture = reservation.Trip.DepartureTime - DateTime.UtcNow;
            if (timeUntilDeparture < CancellationDeadline)
                throw new ConflictException(ResourceMessagesException.CANCELLATION_DEADLINE_EXCEEDED);

            reservation.Status = ReservationStatus.Cancelled;

            _repository.Update(reservation);

            await _unitOfWork.Commit();
        }
    }
}
