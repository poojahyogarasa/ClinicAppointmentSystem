namespace Clinic.App.Services
{
    public class AuthService : IAuthService
    {
        public async System.Threading.Tasks.Task<bool> AuthenticateAsync(string username, string password)
        {
            await System.Threading.Tasks.Task.Delay(500);
            return username == "admin" && password == "admin";
        }
    }
}
