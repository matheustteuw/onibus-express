using OniBusExpress.Domain.Repositories;

namespace OniBusExpress.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OniBusExpressDbContext _dbContext;

        public UnitOfWork(OniBusExpressDbContext dbContext) => _dbContext = dbContext;

        public async Task Commit() => await _dbContext.SaveChangesAsync();
    }
}
