using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;
using Entities.Pagination;

namespace BusinessLogic
{
    public class CarService : BaseService<Car>, ICarService
    {
        public CarService(ICarRepository repository, IMapperService mapperService) : base(repository, mapperService)
        {
        }
        public override PagedList<TDto> GetByPage<TDto>(PaginationQueryParameters parameters)
        {
            var cars = _repository
                .GetAllWithDependencies()
                .Skip((parameters.page - 1) * parameters.pageSize)
                .Take(parameters.pageSize);

            var count = _repository.Count();

            var carDtos = _mapperService.Map<IQueryable<Car>, IEnumerable<TDto>>(cars);

            return new PagedList<TDto>(carDtos.ToList(), count, parameters.page, parameters.pageSize);
        }

        public override PagedList<TDto> GetByPageWithConditions<TDto>(PaginationQueryParameters parameters, Func<Car, bool> condition)
        {
            var cars = _repository
                .GetAllWithDependencies()
                .Where(condition)
                .Skip((parameters.page - 1) * parameters.pageSize)
                .Take(parameters.pageSize);

            var count = _repository.Count();

            var carDtos = _mapperService.Map<IEnumerable<Car>, IEnumerable<TDto>>(cars);

            return new PagedList<TDto>(carDtos.ToList(), count, parameters.page, parameters.pageSize);
        }
    }
}
