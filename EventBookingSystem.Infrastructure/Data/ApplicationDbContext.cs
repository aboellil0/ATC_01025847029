using EventBookingSystem.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");
            modelBuilder.Entity<Booking>().ToTable("Bookings");
            modelBuilder.Entity<Event>().ToTable("Events");

            modelBuilder.ApplyConfiguration(new Config.ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration(new Config.ApplicationRoleConfiguration());
            modelBuilder.ApplyConfiguration(new Config.RefreshTokenConfigration());
            modelBuilder.ApplyConfiguration(new Config.EventConfiguration());
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Event> Events { get; set; }


    }
}
