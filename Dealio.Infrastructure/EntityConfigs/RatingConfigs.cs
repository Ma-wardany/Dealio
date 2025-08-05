using Dealio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dealio.Infrastructure.EntityConfigs
{
    public class RatingConfigs : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            // Primary Key
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Comment)
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            builder.Property(r => r.RatingValue)
                .HasColumnType("int")
                .IsRequired();
        }
    }
}
