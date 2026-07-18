using Microsoft.EntityFrameworkCore;
using OniBusExpress.Domain.Entities;
using OniBusExpress.Domain.Repositories.Trip;

namespace OniBusExpress.Infrastructure.DataAccess.Repositories
{
    public class TripRepository : ITripReadOnlyRepository
    {
        private readonly OniBusExpressDbContext _dbContext;

        public TripRepository(OniBusExpressDbContext dbContext) => _dbContext = dbContext;

        public async Task<IList<Trip>> Search(string origin, string destination, DateOnly departureDate)
        {
            var startOfDay = departureDate.ToDateTime(TimeOnly.MinValue);
            var endOfDay = startOfDay.AddDays(1);

            return await _dbContext
                .Trips
                .AsNoTracking()
                .Include(trip => trip.Route)
                .Include(trip => trip.Reservations)
                .Where(trip => trip.Route.Origin == origin
                            && trip.Route.Destination == destination
                            && trip.DepartureTime >= startOfDay
                            && trip.DepartureTime < endOfDay)
                .OrderBy(trip => trip.DepartureTime)
                .ToListAsync();
        }

        public async Task<Trip?> GetById(Guid tripId)
        {
            return await _dbContext
                .Trips
                .AsNoTracking()
                .Include(trip => trip.Route)
                .Include(trip => trip.Reservations)
                .FirstOrDefaultAsync(trip => trip.Id == tripId);
        }
    }
}
