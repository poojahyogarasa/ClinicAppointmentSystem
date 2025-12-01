namespace Clinic.App.Services
{
    public class AuthService : IAuthService
    {
        // simple demo creds; switch to DB later if you wish
        public bool Login(string username, string password)
            => username == "admin" && password == "1234";
    }
}
