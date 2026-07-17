namespace OniBusExpress.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
