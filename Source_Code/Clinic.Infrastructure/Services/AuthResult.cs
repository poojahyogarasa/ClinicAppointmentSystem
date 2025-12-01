namespace Clinic.App.Services
{
    public sealed class AuthResult
    {
        public bool Success { get; }
        public string? Error { get; }
        public string? DisplayName { get; }
        public string? Role { get; }
        private AuthResult(bool success, string? error, string? displayName, string? role)
        { Success = success; Error = error; DisplayName = displayName; Role = role; }
        public static AuthResult Ok(string displayName, string role) => new(true, null, displayName, role);
        public static AuthResult Fail(string error) => new(false, error, null, null);
    }
}
