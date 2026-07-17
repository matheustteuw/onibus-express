using FluentValidation;
using OniBusExpress.Communication.Requests;
using OniBusExpress.Domain.Extensions;
using OniBusExpress.Exceptions;

namespace OniBusExpress.Application.UseCases.Reservation.Register
{
    public class RegisterReservationValidator : AbstractValidator<RequestRegisterReservationJson>
    {
        public RegisterReservationValidator()
        {
            RuleFor(request => request.PassengerName).NotEmpty().WithMessage(ResourceMessagesException.NAME_EMPTY);
            RuleFor(request => request.TripId).NotEmpty().WithMessage(ResourceMessagesException.TRIP_ID_EMPTY);
            RuleFor(request => request.SeatNumber).GreaterThan(0).WithMessage(ResourceMessagesException.SEAT_NUMBER_INVALID);
            RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceMessagesException.EMAIL_EMPTY);
            When(request => string.IsNullOrWhiteSpace(request.Email) == false, () =>
            {
                RuleFor(request => request.Email)
                    .EmailAddress().WithMessage(ResourceMessagesException.EMAIL_INVALID);
            });
            RuleFor(request => request.Cpf).NotEmpty().WithMessage(ResourceMessagesException.CPF_EMPTY);
            When(request => string.IsNullOrWhiteSpace(request.Cpf) == false, () =>
            {
                RuleFor(request => request.Cpf)
                    .Must(cpf => cpf.IsValidCpf()).WithMessage(ResourceMessagesException.CPF_INVALID);
            });
        }
    }
}
