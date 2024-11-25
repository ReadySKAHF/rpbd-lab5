using Contracts.Repositories;
using DbAccess.Repositories.Base;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories
{
    public class CarRepository : RepositoryBase<Car>, ICarRepository
    {
        public CarRepository(Context.Context context) : base(context) { }

        public override IQueryable<Car> GetAllWithDependencies() =>
            _context.Cars
                .AsNoTracking()
                .Include(c => c.Owner);
    }
}
