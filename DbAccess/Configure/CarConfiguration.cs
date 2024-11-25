using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbAccess.Configure
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder
                .Property(c => c.LicensePlate)
                .IsRequired();

            builder
                .Property(c => c.Brand)
                .IsRequired();

            builder
                .Property(c => c.Power)
                .IsRequired();

            builder
                .Property(c => c.Color)
                .IsRequired();

            builder
                .Property(c => c.YearOfProduction)
                .IsRequired();

            builder
                .Property(c => c.ChassisNumber)
                .IsRequired();

            builder
                .Property(c => c.EngineNumber)
                .IsRequired();

            builder
                .Property(c => c.DateReceived)
                .IsRequired();

            builder
                .HasOne(c => c.Owner)
                .WithMany(o => o.Cars)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
