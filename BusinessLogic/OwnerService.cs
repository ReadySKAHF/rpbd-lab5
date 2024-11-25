using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;
using Entities.Exceptions;
using Entities.Pagination;

namespace BusinessLogic
{
    public class OwnerService : BaseService<Owner>, IOwnerService
    {
        private readonly ICarRepository _carRepository;
        public OwnerService(IOwnerRepository repository, ICarRepository carRepository, IMapperService mapperService) : base(repository, mapperService)
        {
            _carRepository = carRepository;
        }
        public override async Task DeleteByIdAsync(Guid id)
        {
            var transaction = await _repository.CreateTransactionAsync();

            var owner = await _repository.FindByIdAsync(id);

            if (owner == null)
                throw new NotFoundException("Owner not found.");

            var cars = _carRepository.FindByCondition(c => c.OwnerId == owner.Id).ToArray();

            for (var i = 0; i < cars.Length; i++)
            {
                if (cars[i] == null)
                    continue;

                await _carRepository.DeleteAsync(cars[i]);
            }

            await _repository.SaveChangesAsync();

            await _repository.DeleteAsync(owner);

            await transaction.CommitAsync();
        }

        public override PagedList<TDto> GetByPageWithConditions<TDto>(PaginationQueryParameters parameters, Func<Owner, bool> condition)
        {
            var owners = _repository
                .GetAllWithDependencies()
                .Where(condition)
                .Skip((parameters.page - 1) * parameters.pageSize)
                .Take(parameters.pageSize);

            var count = _repository.Count();

            var carDtos = _mapperService.Map<IEnumerable<Owner>, IEnumerable<TDto>>(owners);

            return new PagedList<TDto>(carDtos.ToList(), count, parameters.page, parameters.pageSize);
        }
    }
}
