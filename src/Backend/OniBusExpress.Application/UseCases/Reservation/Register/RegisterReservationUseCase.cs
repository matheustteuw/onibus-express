using AutoMapper;
using OniBusExpress.Communication.Requests;
using OniBusExpress.Communication.Responses;
using OniBusExpress.Domain.Enums;
using OniBusExpress.Domain.Extensions;
using OniBusExpress.Domain.Repositories;
using OniBusExpress.Domain.Repositories.Passenger;
using OniBusExpress.Domain.Repositories.Reservation;
using OniBusExpress.Domain.Repositories.Trip;
using OniBusExpress.Domain.Services.EmailService;
using OniBusExpress.Domain.Services.ReservationCodeGenerator;
using OniBusExpress.Exceptions;
using OniBusExpress.Exceptions.ExceptionsBase;
using Microsoft.Extensions.Logging;

namespace OniBusExpress.Application.UseCases.Reservation.Register
{
    public class RegisterReservationUseCase : IRegisterReservationUseCase
    {
        private readonly ITripReadOnlyRepository _tripRepository;
        private readonly IReservationReadOnlyRepository _reservationReadRepository;
        private readonly IReservationWriteOnlyRepository _reservationWriteRepository;
        private readonly IPassengerReadOnlyRepository _passengerReadRepository;
        private readonly IPassengerWriteOnlyRepository _passengerWriteRepository;
        private readonly IReservationCodeGenerator _codeGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<RegisterReservationUseCase> _logger;

        public RegisterReservationUseCase(
            ITripReadOnlyRepository tripRepository,
            IReservationReadOnlyRepository reservationReadRepository,
            IReservationWriteOnlyRepository reservationWriteRepository,
            IPassengerReadOnlyRepository passengerReadRepository,
            IPassengerWriteOnlyRepository passengerWriteRepository,
            IReservationCodeGenerator codeGenerator,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmailService emailService,
            ILogger<RegisterReservationUseCase> logger)
        {
            _tripRepository = tripRepository;
            _reservationReadRepository = reservationReadRepository;
            _reservationWriteRepository = reservationWriteRepository;
            _passengerReadRepository = passengerReadRepository;
            _passengerWriteRepository = passengerWriteRepository;
            _codeGenerator = codeGenerator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<ResponseRegisteredReservationJson> Execute(RequestRegisterReservationJson request)
        {
            await Validate(request);

            var trip = await _tripRepository.GetById(request.TripId)
                ?? throw new NotFoundException(ResourceMessagesException.TRIP_NOT_FOUND);

            if (trip.DepartureTime <= DateTime.UtcNow)
                throw new ConflictException(ResourceMessagesException.TRIP_ALREADY_DEPARTED);

            if (request.SeatNumber > trip.TotalSeats)
                throw new ErrorOnValidationException([ResourceMessagesException.SEAT_OUT_OF_RANGE]);

            var seatIsTaken = await _reservationReadRepository.ExistsActiveReservationForSeat(trip.Id, request.SeatNumber);
            if (seatIsTaken)
                throw new ConflictException(ResourceMessagesException.SEAT_ALREADY_TAKEN);

            var passengerId = await ResolvePassenger(request);

            var passengerAlreadyHasReservation = await _reservationReadRepository.ExistsActiveReservationForPassenger(trip.Id, passengerId);
            if (passengerAlreadyHasReservation)
                throw new ConflictException(ResourceMessagesException.PASSENGER_ALREADY_HAS_RESERVATION_ON_TRIP);

            var reservation = new Domain.Entities.Reservation
            {
                Id = Guid.NewGuid(),
                ReservationCode = await GenerateUniqueReservationCode(),
                TripId = trip.Id,
                PassengerId = passengerId,
                SeatNumber = request.SeatNumber,
                Status = ReservationStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            await _reservationWriteRepository.Add(reservation);

            await _unitOfWork.Commit();

            await SendConfirmationEmail(request.Email, request.PassengerName, reservation, trip);

            return _mapper.Map<ResponseRegisteredReservationJson>(reservation);
        }

        private async Task<Guid> ResolvePassenger(RequestRegisterReservationJson request)
        {
            var cpf = request.Cpf.OnlyDigits();

            var existingPassenger = await _passengerReadRepository.GetByCpf(cpf);
            if (existingPassenger is not null)
                return existingPassenger.Id;

            var passenger = _mapper.Map<Domain.Entities.Passenger>(request);
            passenger.Id = Guid.NewGuid();

            await _passengerWriteRepository.Add(passenger);

            return passenger.Id;
        }

        private async Task SendConfirmationEmail(string toEmail, string passengerName, Domain.Entities.Reservation reservation, Domain.Entities.Trip trip)
        {
            try
            {
                await _emailService.SendReservationConfirmation(
                    toEmail,
                    passengerName,
                    reservation.ReservationCode,
                    trip.Route.Origin,
                    trip.Route.Destination,
                    trip.DepartureTime,
                    reservation.SeatNumber);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Falha ao enviar e-mail de confirmação da reserva {ReservationCode} para {Email}", reservation.ReservationCode, toEmail);
            }
        }

        private async Task<string> GenerateUniqueReservationCode()
        {
            string code;

            do
            {
                code = _codeGenerator.Generate();
            }
            while (await _reservationReadRepository.ExistsReservationWithCode(code));

            return code;
        }

        private static async Task Validate(RequestRegisterReservationJson request)
        {
            var result = await new RegisterReservationValidator().ValidateAsync(request);

            if (result.IsValid == false)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

                throw new ErrorOnValidationException(errorMessages);
            }
        }
    }
}
