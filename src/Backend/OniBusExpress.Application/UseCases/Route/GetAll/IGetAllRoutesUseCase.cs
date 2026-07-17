using OniBusExpress.Communication.Responses;

namespace OniBusExpress.Application.UseCases.Route.GetAll
{
    public interface IGetAllRoutesUseCase
    {
        Task<ResponseRoutesJson> Execute();
    }
}
