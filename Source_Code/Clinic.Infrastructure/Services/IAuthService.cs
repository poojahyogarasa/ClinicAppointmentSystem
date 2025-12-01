namespace Clinic.App.Services
{
    public interface IAuthService
    {
        System.Threading.Tasks.Task<bool> AuthenticateAsync(string username, string password);
    }
}
