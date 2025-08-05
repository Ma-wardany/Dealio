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
    public class SellerTransactionConfigs : IEntityTypeConfiguration<SellerTransaction>
    {
        public void Configure(EntityTypeBuilder<SellerTransaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TransferStatus)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            builder.Property(t => t.AmountReceived)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(t => t.Seller)
                .WithMany(s => s.SellerTransactions)
                .HasForeignKey(t => t.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Order)
                .WithOne(o => o.SellerTransaction)
                .HasForeignKey<SellerTransaction>(t => t.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
