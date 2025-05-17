using EventBookingSystem.Core.Entities;
using EventBookingSystem.Core.Interfaces.Repositories;
using EventBookingSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBookingSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _manager;

        public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> CheckUsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<ApplicationUser> AddUserAsync(ApplicationUser user, string pass)
        {
            var result = await _manager.CreateAsync(user, pass);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // IMPORTANT: The user must be saved to the database BEFORE adding to role
            // Add to role after successful user creation
            var roleResult = await _manager.AddToRoleAsync(user, "User"); // Or whatever role you're using
            if (!roleResult.Succeeded)
            {
                // Handle role assignment failure
                throw new Exception($"User created but role assignment failed: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
            return user; 
        }

        public async Task<ApplicationUser> AddAdminAsync(ApplicationUser user, string pass)
        {
            var result = await _manager.CreateAsync(user, pass);
            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create Admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // IMPORTANT: The user must be saved to the database BEFORE adding to role
            // Add to role after successful user creation
            var roleResult = await _manager.AddToRoleAsync(user, "Admin"); // Or whatever role you're using
            if (!roleResult.Succeeded)
            {
                // Handle role assignment failure
                throw new Exception($"Admin created but role assignment failed: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
            return user;
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(ApplicationUser user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(e => e.Id == userId);
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetUserRolesAsync(Guid userId)
        {
            var user = await _manager.FindByIdAsync(userId.ToString());
            var roles = await _manager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> AssignRoleToUserAsync(Guid userId, string roleName)
        {
            var user = await _manager.FindByIdAsync(userId.ToString());
            var result = await _manager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
