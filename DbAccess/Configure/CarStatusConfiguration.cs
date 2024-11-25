using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbAccess.Configure
{
    public class CarStatusConfiguration : IEntityTypeConfiguration<CarStatus>
    {
        public void Configure(EntityTypeBuilder<CarStatus> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(s => s.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder
                .Property(c => c.StatusName)
                .IsRequired();

        }
    }
}