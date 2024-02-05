using Products.Module.Models;

namespace Products.Module;

public interface IUserRepository
{
    Task<User?> GetUserByApiKey(Guid apiKey);
}