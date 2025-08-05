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
    public class ApplicationUserConfigs : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne(a => a.UserProfile)
                .WithOne(u => u.ApplicationUser)
                .HasForeignKey<UserProfile>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(a => a.DeliveryProfile)
                .WithOne(d => d.ApplicationUser)
                .HasForeignKey<DeliveryProfile>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(a => a.Address)
                .WithOne(ad => ad.ApplicationUser)
                .HasForeignKey<Address>(a => a.UserId)
                .IsRequired();
        }
    }
}
