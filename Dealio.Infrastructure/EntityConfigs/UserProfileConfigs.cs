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
    public class UserProfileConfigs : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasKey(u => u.UserId);

            builder.Property(u => u.FirstName)
                .HasColumnType("NVARCHAR(20)")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.LastName)
                .HasColumnType("NVARCHAR(20)")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.Image)
                .IsRequired(false);

            builder.Property(u => u.Phone)
                .HasColumnType("NVARCHAR(11)")
                .HasMaxLength(11)
                .IsRequired();

            builder.HasMany(u => u.Products)
                .WithOne(p => p.Seller)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(u => u.orders)
                .WithOne(o => o.Buyer)
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

        }
    }
}
