using Microsoft.EntityFrameworkCore;
using Clinic.Domain.Entities;

namespace Clinic.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Patient> Patients => Set<Patient>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Patient config
            modelBuilder.Entity<Patient>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Name).HasMaxLength(120).IsRequired();
                e.Property(p => p.Phone).HasMaxLength(30);
                e.Property(p => p.Email).HasMaxLength(160);
                e.Property(p => p.Address).HasMaxLength(200);
            });

            // Seed (optional)
            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, Name = "A. Perera", Phone = "0771234567", Email = "a.perera@mail.com", Address = "Colombo" },
                new Patient { Id = 2, Name = "B. Silva", Phone = "0772345678", Email = "b.silva@mail.com", Address = "Galle" }
            );
        }
    }
}
