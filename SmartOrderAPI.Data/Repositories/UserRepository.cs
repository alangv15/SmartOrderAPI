using Microsoft.EntityFrameworkCore;
using SmartOrderAPI.Data.Mappers;
using SmartOrderAPI.Data.Models;
using SmartOrderAPI.Entities.Models;
using System.Security.Cryptography;
using System.Text;

namespace SmartOrderAPI.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SmartOrderContext _context;

        public UserRepository(SmartOrderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .Include(user => user.Role)
                .OrderByDescending(user => user.IsActive)
                .ThenBy(user => user.FullName)
                .Select(user => user.ToDto())
                .ToListAsync();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(user => user.Role)
                .Where(user => user.UserId == id)
                .Select(user => user.ToDto())
                .FirstOrDefaultAsync();
        }

        public async Task<UserDto?> ValidateUserAsync(string email, string password)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
            if (user == null)
            {
                return null;
            }

            var computedHash = ComputeHash(password, user.PasswordSalt);
            if (!SecureEquals(computedHash, user.PasswordHash))
            {
                return null;
            }

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return user.ToDto();
        }

        public async Task<List<string>> GetPermissionCodesByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
                .AsNoTracking()
                .Where(rolePermission => rolePermission.RoleId == roleId && rolePermission.Permission.IsActive)
                .Select(rolePermission => rolePermission.Permission.Code)
                .Distinct()
                .OrderBy(code => code)
                .ToListAsync();
        }

        public async Task AddAsync(UserDto user)
        {
            if (await _context.Users.AnyAsync(existingUser => existingUser.Email == user.Email))
            {
                throw new ArgumentException("El correo ya está registrado.");
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentException("La contraseña es obligatoria.");
            }

            var saltBytes = RandomNumberGenerator.GetBytes(64);
            var salt = Convert.ToBase64String(saltBytes);
            var hash = ComputeHash(user.Password, salt);

            var newUser = new User
            {
                FullName = user.FullName,
                Email = user.Email,
                PasswordSalt = salt,
                PasswordHash = hash,
                RoleId = user.RoleId,
                CreatedAt = DateTime.UtcNow,
                IsActive = user.IsActive
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserDto dto)
        {
            var existing = await _context.Users.FindAsync(dto.UserId);
            if (existing == null)
            {
                return;
            }

            existing.FullName = dto.FullName;
            existing.Email = dto.Email;
            existing.RoleId = dto.RoleId;
            existing.IsActive = dto.IsActive;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var saltBytes = RandomNumberGenerator.GetBytes(64);
                var salt = Convert.ToBase64String(saltBytes);
                existing.PasswordSalt = salt;
                existing.PasswordHash = ComputeHash(dto.Password, salt);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }

        public Task<bool> RoleExistsAsync(int roleId)
        {
            return _context.Roles.AnyAsync(role => role.RoleId == roleId && role.IsActive);
        }

        private static bool SecureEquals(string a, string b)
        {
            var aBytes = Encoding.UTF8.GetBytes(a);
            var bBytes = Encoding.UTF8.GetBytes(b);
            return CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
        }

        private static string ComputeHash(string password, string salt)
        {
            using var hmac = new HMACSHA512(Convert.FromBase64String(salt));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }
    }
}
