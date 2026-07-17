using AutoMapper;
using FluentAssertions;
using Moq;
using OniBusExpress.Application.Services.AutoMapper;
using OniBusExpress.Application.UseCases.Reservation.Register;
using OniBusExpress.Communication.Requests;
using OniBusExpress.Domain.Entities;
using OniBusExpress.Domain.Repositories;
using OniBusExpress.Domain.Repositories.Passenger;
using OniBusExpress.Domain.Repositories.Reservation;
using OniBusExpress.Domain.Repositories.Trip;
using OniBusExpress.Domain.Services.ReservationCodeGenerator;
using OniBusExpress.Exceptions;
using OniBusExpress.Exceptions.ExceptionsBase;
using ReservationEntity = OniBusExpress.Domain.Entities.Reservation;

namespace OniBusExpress.Tests.UseCases.Reservation.Register
{
    public class RegisterReservationUseCaseTest
    {
        private readonly Mock<ITripReadOnlyRepository> _tripRepository = new();
        private readonly Mock<IReservationReadOnlyRepository> _reservationReadRepository = new();
        private readonly Mock<IReservationWriteOnlyRepository> _reservationWriteRepository = new();
        private readonly Mock<IPassengerReadOnlyRepository> _passengerReadRepository = new();
        private readonly Mock<IPassengerWriteOnlyRepository> _passengerWriteRepository = new();
        private readonly Mock<IReservationCodeGenerator> _codeGenerator = new();
        private readonly Mock<IUnitOfWork> _unitOfWork = new();
        private readonly IMapper _mapper;

        public RegisterReservationUseCaseTest()
        {
            _mapper = new MapperConfiguration(config => config.AddProfile(new AutoMapping())).CreateMapper();

            _reservationWriteRepository.Setup(repository => repository.Add(It.IsAny<ReservationEntity>())).Returns(Task.CompletedTask);
            _passengerWriteRepository.Setup(repository => repository.Add(It.IsAny<Passenger>())).Returns(Task.CompletedTask);
            _unitOfWork.Setup(unitOfWork => unitOfWork.Commit()).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task Execute_Should_Throw_ConflictException_When_Seat_Already_Taken()
        {
            var trip = BuildTrip();
            var request = BuildRequest(trip.Id, seatNumber: 5);

            _tripRepository.Setup(repository => repository.GetById(trip.Id)).ReturnsAsync(trip);
            _reservationReadRepository.Setup(repository => repository.ExistsActiveReservationForSeat(trip.Id, 5)).ReturnsAsync(true);

            var useCase = CreateUseCase();

            var act = async () => await useCase.Execute(request);

            var exception = await act.Should().ThrowAsync<ConflictException>();
            exception.Which.GetErrorMessages().Should().Contain(ResourceMessagesException.SEAT_ALREADY_TAKEN);
        }

        [Fact]
        public async Task Execute_Should_Retry_Code_Generation_When_Generated_Code_Already_Exists()
        {
            var trip = BuildTrip();
            var request = BuildRequest(trip.Id, seatNumber: 5);

            _tripRepository.Setup(repository => repository.GetById(trip.Id)).ReturnsAsync(trip);
            _reservationReadRepository.Setup(repository => repository.ExistsActiveReservationForSeat(trip.Id, 5)).ReturnsAsync(false);
            _reservationReadRepository.Setup(repository => repository.ExistsActiveReservationForPassenger(trip.Id, It.IsAny<Guid>())).ReturnsAsync(false);

            _codeGenerator
                .SetupSequence(generator => generator.Generate())
                .Returns("ABC-11111")
                .Returns("ABC-22222");

            _reservationReadRepository.Setup(repository => repository.ExistsReservationWithCode("ABC-11111")).ReturnsAsync(true);
            _reservationReadRepository.Setup(repository => repository.ExistsReservationWithCode("ABC-22222")).ReturnsAsync(false);

            var useCase = CreateUseCase();

            var result = await useCase.Execute(request);

            result.ReservationCode.Should().Be("ABC-22222");
            _codeGenerator.Verify(generator => generator.Generate(), Times.Exactly(2));
        }

        private RegisterReservationUseCase CreateUseCase()
        {
            return new RegisterReservationUseCase(
                _tripRepository.Object,
                _reservationReadRepository.Object,
                _reservationWriteRepository.Object,
                _passengerReadRepository.Object,
                _passengerWriteRepository.Object,
                _codeGenerator.Object,
                _unitOfWork.Object,
                _mapper);
        }

        private static Trip BuildTrip()
        {
            return new Trip
            {
                Id = Guid.NewGuid(),
                DepartureTime = DateTime.UtcNow.AddDays(1),
                BasePrice = 100m,
                TotalSeats = 40
            };
        }

        private static RequestRegisterReservationJson BuildRequest(Guid tripId, int seatNumber)
        {
            return new RequestRegisterReservationJson
            {
                TripId = tripId,
                SeatNumber = seatNumber,
                PassengerName = "Maria Souza",
                Cpf = "111.444.777-35",
                Email = "maria.souza@example.com"
            };
        }
    }
}
