using Dealio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dealio.Infrastructure.EntityConfigs
{
    public class OrderConfigs : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Primary Key
            builder.HasKey(o => o.Id);

            // OrderDate - Required
            builder.Property(o => o.OrderDate)
                .IsRequired();

            // OrderStatus - Optional or you can enforce max length if needed
            builder.Property(o => o.OrderStatus)
                .IsRequired();

            // OrderNumber - Required, unique
            builder.Property(o => o.OrderNumber)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();

            builder.HasOne(o => o.Product)
                .WithOne(p => p.Order)
                .HasForeignKey<Order>(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
