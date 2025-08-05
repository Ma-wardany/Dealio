using Dealio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Dealio.Infrastructure.EntityConfigs
{
    public class DeliveryProileConfigs : IEntityTypeConfiguration<DeliveryProfile>
    {
        public void Configure(EntityTypeBuilder<DeliveryProfile> builder)
        {
            builder.HasKey(d => d.UserId);


            builder.Property(d => d.FirstName)
                .HasColumnType("NVARCHAR(20)")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(d => d.LastName)
                .HasColumnType("NVARCHAR(20)")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(d => d.Image)
                .IsRequired();

            builder.Property(u => u.Phone)
                .HasColumnType("NVARCHAR(11)")
                .HasMaxLength(11)
                .IsRequired();


            builder.Property(u => u.NationalId)
                .HasColumnType("NVARCHAR(14)")
                .HasMaxLength(14)
                .IsRequired();


            builder.HasMany(d => d.Orders)
                .WithOne(o => o.Delivery)
                .HasForeignKey(o => o.DeliveryId)
                .IsRequired(false);

        }
    }
}
