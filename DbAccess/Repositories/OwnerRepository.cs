using Contracts.Repositories;
using DbAccess.Repositories.Base;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(Context.Context context) : base(context) { }

        public override IQueryable<Owner> GetAllWithDependencies() =>
            _context.Owners
                .AsNoTracking();
    }
}
