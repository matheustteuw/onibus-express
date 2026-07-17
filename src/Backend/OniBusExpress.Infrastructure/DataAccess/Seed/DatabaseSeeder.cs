using Microsoft.EntityFrameworkCore;
using OniBusExpress.Domain.Entities;

namespace OniBusExpress.Infrastructure.DataAccess.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(OniBusExpressDbContext dbContext)
        {
            if (await dbContext.Routes.AnyAsync())
                return;

            var saoPauloRioDeJaneiro = new Route
            {
                Id = Guid.NewGuid(),
                Origin = "São Paulo",
                Destination = "Rio de Janeiro",
                EstimatedDuration = TimeSpan.FromHours(6)
            };

            var rioDeJaneiroSaoPaulo = new Route
            {
                Id = Guid.NewGuid(),
                Origin = "Rio de Janeiro",
                Destination = "São Paulo",
                EstimatedDuration = TimeSpan.FromHours(6)
            };

            var saoPauloBeloHorizonte = new Route
            {
                Id = Guid.NewGuid(),
                Origin = "São Paulo",
                Destination = "Belo Horizonte",
                EstimatedDuration = TimeSpan.FromHours(8)
            };

            var routes = new[] { saoPauloRioDeJaneiro, rioDeJaneiroSaoPaulo, saoPauloBeloHorizonte };

            await dbContext.Routes.AddRangeAsync(routes);

            var today = DateTime.UtcNow.Date;

            var trips = new[]
            {
                new Trip
                {
                    Id = Guid.NewGuid(),
                    RouteId = saoPauloRioDeJaneiro.Id,
                    DepartureTime = today.AddDays(1).AddHours(8),
                    BasePrice = 120.00m,
                    TotalSeats = 40
                },
                new Trip
                {
                    Id = Guid.NewGuid(),
                    RouteId = saoPauloRioDeJaneiro.Id,
                    DepartureTime = today.AddDays(1).AddHours(22),
                    BasePrice = 135.00m,
                    TotalSeats = 40
                },
                new Trip
                {
                    Id = Guid.NewGuid(),
                    RouteId = rioDeJaneiroSaoPaulo.Id,
                    DepartureTime = today.AddDays(2).AddHours(9),
                    BasePrice = 120.00m,
                    TotalSeats = 40
                },
                new Trip
                {
                    Id = Guid.NewGuid(),
                    RouteId = saoPauloBeloHorizonte.Id,
                    DepartureTime = today.AddDays(3).AddHours(7),
                    BasePrice = 150.00m,
                    TotalSeats = 36
                }
            };

            await dbContext.Trips.AddRangeAsync(trips);

            await dbContext.SaveChangesAsync();
        }
    }
}
