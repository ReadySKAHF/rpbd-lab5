using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;
using Entities.Pagination;

namespace BusinessLogic
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IUserRepository repository, ICarStatusRepository settlementRepository, IMapperService mapperService) : base(repository, mapperService)
        {
        }

        public override PagedList<TDto> GetByPageWithConditions<TDto>(PaginationQueryParameters parameters, Func<User, bool> condition)
        {
            var users = _repository
                .GetAllWithDependencies()
                .Where(condition)
                .Skip((parameters.page - 1) * parameters.pageSize)
                .Take(parameters.pageSize);

            var count = _repository.Count();

            var carDtos = _mapperService.Map<IEnumerable<User>, IEnumerable<TDto>>(users);

            return new PagedList<TDto>(carDtos.ToList(), count, parameters.page, parameters.pageSize);
        }
    }
}
