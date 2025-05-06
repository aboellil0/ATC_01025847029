using EventBookingSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Data.Config
{
    public partial class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.Property(r => r.Id).ValueGeneratedOnAdd();
            
            
            builder.HasData(
            new
            {
                Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3301"),
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "Administrator role with full access",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) // Static DateTime
            },
            new
            {
                Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3302"),
                Name = "User",
                NormalizedName = "USER",
                Description = "Regular user role with limited access",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) // Static DateTime
            },
            new
            {
                Id = Guid.Parse("3F2504E0-4F89-41D3-9A0C-0305E82C3303"),
                Name = "EventManager",
                NormalizedName = "EVENTMANAGER",
                Description = "Role for managing events",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) // Static DateTime
            });

            builder.HasMany(r => r.Users)
                .WithOne()
                .HasForeignKey("RoleId")
                .IsRequired();
        }

    }
}
