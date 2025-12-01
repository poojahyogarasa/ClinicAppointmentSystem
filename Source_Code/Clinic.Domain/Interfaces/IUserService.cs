using System.Threading.Tasks;

namespace Clinic.Domain.Interfaces;

public interface IUserService
{
    Task<bool> ValidateAsync(string username, string password);
    Task SeedIfEmptyAsync();
}
