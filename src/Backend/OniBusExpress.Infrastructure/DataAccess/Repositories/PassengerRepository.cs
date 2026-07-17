using Microsoft.EntityFrameworkCore;
using OniBusExpress.Domain.Repositories.Passenger;

namespace OniBusExpress.Infrastructure.DataAccess.Repositories
{
    public class PassengerRepository : IPassengerReadOnlyRepository, IPassengerWriteOnlyRepository
    {
        private readonly OniBusExpressDbContext _dbContext;

        public PassengerRepository(OniBusExpressDbContext dbContext) => _dbContext = dbContext;

        public async Task<Domain.Entities.Passenger?> GetByCpf(string cpf)
        {
            return await _dbContext
                .Passengers
                .AsNoTracking()
                .FirstOrDefaultAsync(passenger => passenger.Cpf == cpf);
        }

        public async Task Add(Domain.Entities.Passenger passenger)
        {
            await _dbContext.Passengers.AddAsync(passenger);
        }
    }
}
