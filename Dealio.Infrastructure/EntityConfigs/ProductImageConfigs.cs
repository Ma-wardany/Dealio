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
    public class ProductImageConfigs : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Product)
                .WithMany(pi => pi.Images)
                .HasForeignKey(p => p.ProductId)
                .IsRequired();
        }
    }
}
