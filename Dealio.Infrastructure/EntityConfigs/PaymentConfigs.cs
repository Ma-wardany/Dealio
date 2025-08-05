using Dealio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dealio.Infrastructure.EntityConfigs
{
    public class PaymentConfigs : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PaymentStatus)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            builder.Property(p => p.CardInfo)
                .HasColumnType("nvarchar(100)")
                .IsRequired(false);

            builder.HasOne(p => p.Buyer)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.OrderId).IsUnique();
        }
    }
}
