namespace OniBusExpress.Domain.Repositories.Route
{
    public interface IRouteReadOnlyRepository
    {
        Task<IList<Entities.Route>> GetAll();
    }
}
