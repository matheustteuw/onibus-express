using AutoMapper;
using OniBusExpress.Communication.Responses;
using OniBusExpress.Domain.Repositories.Route;

namespace OniBusExpress.Application.UseCases.Route.GetAll
{
    public class GetAllRoutesUseCase : IGetAllRoutesUseCase
    {
        private readonly IRouteReadOnlyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllRoutesUseCase(IRouteReadOnlyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseRoutesJson> Execute()
        {
            var routes = await _repository.GetAll();

            return new ResponseRoutesJson
            {
                Routes = _mapper.Map<IList<ResponseShortRouteJson>>(routes)
            };
        }
    }
}
