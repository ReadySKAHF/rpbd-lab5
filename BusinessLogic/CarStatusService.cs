using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;
using Entities.Pagination;

namespace BusinessLogic
{
    public class CarStatusService : BaseService<CarStatus>, ICarStatusService
    {
        public CarStatusService(ICarStatusRepository repository, IMapperService mapperService) : base(repository, mapperService)
        { }

        public override PagedList<TDto> GetByPageWithConditions<TDto>(PaginationQueryParameters parameters, Func<CarStatus, bool> condition)
        {
            var carsStatuses = _repository
                .GetAllWithDependencies()
                .Where(condition)
                .Skip((parameters.page - 1) * parameters.pageSize)
                .Take(parameters.pageSize);

            var count = _repository.Count();

            var carDtos = _mapperService.Map<IEnumerable<CarStatus>, IEnumerable<TDto>>(carsStatuses);

            return new PagedList<TDto>(carDtos.ToList(), count, parameters.page, parameters.pageSize);
        }
    }
}
