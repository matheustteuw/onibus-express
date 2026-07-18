using Microsoft.EntityFrameworkCore;
using OniBusExpress.Domain.Entities;
using OniBusExpress.Domain.Repositories.Route;

namespace OniBusExpress.Infrastructure.DataAccess.Repositories
{
    public class RouteRepository : IRouteReadOnlyRepository
    {
        private readonly OniBusExpressDbContext _dbContext;

        public RouteRepository(OniBusExpressDbContext dbContext) => _dbContext = dbContext;

        public async Task<IList<Route>> GetAll()
        {
            return await _dbContext
                .Routes
                .AsNoTracking()
                .OrderBy(route => route.Origin)
                .ThenBy(route => route.Destination)
                .ToListAsync();
        }
    }
}
