namespace Clinic.Domain.Entities;

public class User
{
    public int Id { get; set; }                // Unique ID for each user
    public string Username { get; set; } = ""; // Login username
    public string PasswordHash { get; set; } = ""; // Hashed password
    public string Role { get; set; } = "Admin"; // "Admin" or "Reception"
    public bool IsActive { get; set; } = true;  // Soft delete / active flag
}
