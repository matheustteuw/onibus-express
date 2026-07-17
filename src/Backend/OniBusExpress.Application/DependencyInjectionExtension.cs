using Microsoft.Extensions.DependencyInjection;
using OniBusExpress.Application.Services.AutoMapper;
using OniBusExpress.Application.UseCases.Reservation.Cancel;
using OniBusExpress.Application.UseCases.Reservation.GetByCode;
using OniBusExpress.Application.UseCases.Reservation.Register;
using OniBusExpress.Application.UseCases.Route.GetAll;
using OniBusExpress.Application.UseCases.Trip.GetById;
using OniBusExpress.Application.UseCases.Trip.Search;

namespace OniBusExpress.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddAutoMapper(services);
            AddUseCases(services);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddScoped(option => new AutoMapper.MapperConfiguration(autoMapperOptions =>
            {
                autoMapperOptions.AddProfile(new AutoMapping());
            }).CreateMapper());
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IGetAllRoutesUseCase, GetAllRoutesUseCase>();
            services.AddScoped<ISearchTripsUseCase, SearchTripsUseCase>();
            services.AddScoped<IGetTripByIdUseCase, GetTripByIdUseCase>();
            services.AddScoped<IRegisterReservationUseCase, RegisterReservationUseCase>();
            services.AddScoped<IGetReservationByCodeUseCase, GetReservationByCodeUseCase>();
            services.AddScoped<ICancelReservationUseCase, CancelReservationUseCase>();
        }
    }
}
