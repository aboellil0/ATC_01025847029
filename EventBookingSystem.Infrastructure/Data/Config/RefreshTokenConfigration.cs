using EventBookingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Data.Config
{
    public partial class RefreshTokenConfigration : IEntityTypeConfiguration<RefreshToken> 
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder
                .HasOne(builder => builder.ApplicationUser)
                .WithMany(builder => builder.refreshTokens)
                .HasForeignKey(builder => builder.UserId);
        }
    }
}
