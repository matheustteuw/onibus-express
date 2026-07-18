using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OniBusExpress.Domain.Repositories;
using OniBusExpress.Domain.Repositories.Passenger;
using OniBusExpress.Domain.Repositories.Reservation;
using OniBusExpress.Domain.Repositories.Route;
using OniBusExpress.Domain.Repositories.Trip;
using OniBusExpress.Domain.Services.EmailService;
using OniBusExpress.Domain.Services.ReservationCodeGenerator;
using OniBusExpress.Infrastructure.DataAccess;
using OniBusExpress.Infrastructure.DataAccess.Repositories;

namespace OniBusExpress.Infrastructure
{
    public static class DependencyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);
            AddServices(services);
            AddEmailService(services);
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<OniBusExpressDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRouteReadOnlyRepository, RouteRepository>();
            services.AddScoped<ITripReadOnlyRepository, TripRepository>();

            services.AddScoped<IPassengerReadOnlyRepository, PassengerRepository>();
            services.AddScoped<IPassengerWriteOnlyRepository, PassengerRepository>();

            services.AddScoped<IReservationReadOnlyRepository, ReservationRepository>();
            services.AddScoped<IReservationWriteOnlyRepository, ReservationRepository>();
            services.AddScoped<IReservationUpdateOnlyRepository, ReservationRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IReservationCodeGenerator, Services.ReservationCodeGenerator.ReservationCodeGenerator>();
        }

        private static void AddEmailService(IServiceCollection services)
        {
            services.AddScoped<IEmailService, Services.EmailService.EmailService>();
        }
    }
}
