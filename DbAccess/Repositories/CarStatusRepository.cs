using Contracts.Repositories;
using DbAccess.Repositories.Base;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories
{
    public class CarStatusRepository : RepositoryBase<CarStatus>, ICarStatusRepository
    {
        public CarStatusRepository(Context.Context context) : base(context) { }

        public override IQueryable<CarStatus> GetAllWithDependencies() =>
            _context.CarsStatuses
                .AsNoTracking();
    }
}
