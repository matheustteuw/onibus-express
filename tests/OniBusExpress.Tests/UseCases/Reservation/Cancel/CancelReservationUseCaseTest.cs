using FluentAssertions;
using Moq;
using OniBusExpress.Application.UseCases.Reservation.Cancel;
using OniBusExpress.Domain.Entities;
using OniBusExpress.Domain.Enums;
using OniBusExpress.Domain.Repositories;
using OniBusExpress.Domain.Repositories.Reservation;
using OniBusExpress.Exceptions;
using OniBusExpress.Exceptions.ExceptionsBase;
using ReservationEntity = OniBusExpress.Domain.Entities.Reservation;

namespace OniBusExpress.Tests.UseCases.Reservation.Cancel
{
    public class CancelReservationUseCaseTest
    {
        private readonly Mock<IReservationUpdateOnlyRepository> _repository = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();

        public CancelReservationUseCaseTest()
        {
            _unitOfWork.Setup(unitOfWork => unitOfWork.Commit()).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task Execute_Should_Cancel_When_Before_Deadline()
        {
            var reservation = BuildReservation(departureTime: DateTime.UtcNow.AddHours(3));

            _repository.Setup(repository => repository.GetByCode(reservation.ReservationCode)).ReturnsAsync(reservation);

            var useCase = CreateUseCase();

            await useCase.Execute(reservation.ReservationCode);

            reservation.Status.Should().Be(ReservationStatus.Cancelled);
            _repository.Verify(repository => repository.Update(reservation), Times.Once);
        }

        [Fact]
        public async Task Execute_Should_Throw_ConflictException_When_Within_Two_Hours_Of_Departure()
        {
            var reservation = BuildReservation(departureTime: DateTime.UtcNow.AddHours(1));

            _repository.Setup(repository => repository.GetByCode(reservation.ReservationCode)).ReturnsAsync(reservation);

            var useCase = CreateUseCase();

            var act = async () => await useCase.Execute(reservation.ReservationCode);

            var exception = await act.Should().ThrowAsync<ConflictException>();
            exception.Which.GetErrorMessages().Should().Contain(ResourceMessagesException.CANCELLATION_DEADLINE_EXCEEDED);

            reservation.Status.Should().Be(ReservationStatus.Active);
        }

        [Fact]
        public async Task Execute_Should_Throw_NotFoundException_When_Reservation_Does_Not_Exist()
        {
            _repository.Setup(repository => repository.GetByCode(It.IsAny<string>())).ReturnsAsync((ReservationEntity?)null);

            var useCase = CreateUseCase();

            var act = async () => await useCase.Execute("ABC-99999");

            await act.Should().ThrowAsync<NotFoundException>();
        }

        private CancelReservationUseCase CreateUseCase() => new(_repository.Object, _unitOfWork.Object);

        private static ReservationEntity BuildReservation(DateTime departureTime)
        {
            var trip = new Trip
            {
                Id = Guid.NewGuid(),
                DepartureTime = departureTime,
                BasePrice = 100m,
                TotalSeats = 40
            };

            return new ReservationEntity
            {
                Id = Guid.NewGuid(),
                ReservationCode = "ABC-12345",
                TripId = trip.Id,
                Trip = trip,
                SeatNumber = 10,
                Status = ReservationStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
