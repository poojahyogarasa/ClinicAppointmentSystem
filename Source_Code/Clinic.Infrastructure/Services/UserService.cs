using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Clinic.Domain.Entities;
using Clinic.Domain.Interfaces;
using Clinic.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    public UserService(AppDbContext db) => _db = db;

    public async Task<bool> ValidateAsync(string username, string password)
    {
        var u = await _db.Users.FirstOrDefaultAsync(x => x.Username == username && x.IsActive);
        if (u is null) return false;
        return u.PasswordHash == Hash(password);
    }

    public async Task SeedIfEmptyAsync()
    {
        await _db.Database.EnsureCreatedAsync();

        if (!await _db.Users.AnyAsync())
        {
            _db.Users.Add(new User
            {
                Username = "admin",
                PasswordHash = Hash("admin123"),
                Role = "Admin",
                IsActive = true
            });
        }

        if (!await _db.Doctors.AnyAsync())
        {
            _db.Doctors.AddRange(
                new Doctor { Name = "Dr. S. Perera", Specialty = "General Physician", Phone = "0771234567" },
                new Doctor { Name = "Dr. N. Fernando", Specialty = "Cardiologist", Phone = "0779876543" }
            );
        }

        if (!await _db.Patients.AnyAsync())
        {
            _db.Patients.AddRange(
                new Patient { FullName = "K. Tharani", Phone = "0711111111", Email = "t@example.com" },
                new Patient { FullName = "R. Manoj", Phone = "0722222222", Email = "m@example.com" }
            );
        }

        await _db.SaveChangesAsync();
    }

    private static string Hash(string input)
    {
        using var sha = SHA256.Create();
        return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
    }
}
