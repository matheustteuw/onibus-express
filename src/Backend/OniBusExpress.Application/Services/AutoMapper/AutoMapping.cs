using AutoMapper;
using OniBusExpress.Communication.Requests;
using OniBusExpress.Communication.Responses;
using OniBusExpress.Domain.Enums;
using OniBusExpress.Domain.Extensions;

namespace OniBusExpress.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterReservationJson, Domain.Entities.Passenger>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.PassengerName))
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(source => source.Cpf.OnlyDigits()));
        }

        private void DomainToResponse()
        {
            CreateMap<Domain.Entities.Route, ResponseShortRouteJson>()
                .ForMember(dest => dest.EstimatedDuration, opt => opt.MapFrom(source => source.EstimatedDuration.ToString(@"hh\:mm")));

            CreateMap<Domain.Entities.Trip, ResponseShortTripJson>()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(source => source.Route.Origin))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(source => source.Route.Destination))
                .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(source =>
                    source.TotalSeats - source.Reservations.Count(reservation => reservation.Status == ReservationStatus.Active)));

            CreateMap<Domain.Entities.Trip, ResponseTripDetailsJson>()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(source => source.Route.Origin))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(source => source.Route.Destination))
                .ForMember(dest => dest.AvailableSeats, opt => opt.MapFrom(source =>
                    source.TotalSeats - source.Reservations.Count(reservation => reservation.Status == ReservationStatus.Active)))
                .ForMember(dest => dest.OccupiedSeats, opt => opt.MapFrom(source =>
                    source.Reservations
                        .Where(reservation => reservation.Status == ReservationStatus.Active)
                        .Select(reservation => reservation.SeatNumber)
                        .OrderBy(seat => seat)
                        .ToList()));

            CreateMap<Domain.Entities.Reservation, ResponseRegisteredReservationJson>();

            CreateMap<Domain.Entities.Reservation, ResponseReservationJson>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(source => source.Status.ToString()))
                .ForMember(dest => dest.PassengerName, opt => opt.MapFrom(source => source.Passenger.Name))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(source => source.Trip.Route.Origin))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(source => source.Trip.Route.Destination))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(source => source.Trip.DepartureTime))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(source => source.Trip.BasePrice));
        }
    }
}
