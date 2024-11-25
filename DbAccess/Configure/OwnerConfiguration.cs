using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbAccess.Configure
{
    public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder
                .HasKey(o => o.Id);

            builder
                .Property(o => o.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder
                .Property(o => o.DriverLicenseNumber)
                .IsRequired();

            builder
                .Property(o => o.FullName)
                .IsRequired();

            builder
                .Property(o => o.Address)
                .IsRequired();

            builder
                .Property(o => o.Phone)
                .IsRequired();

            builder
                .HasMany(o => o.Cars)
                .WithOne(c => c.Owner)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
