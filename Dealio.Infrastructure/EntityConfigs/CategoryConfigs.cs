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
    public class CategoryConfigs : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.Name)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder.Property(c => c.Description)
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);
        }
    }
}
