using Products.Module.Models;

namespace Products.Module;

// this class returns mock data only
internal class UsersRepository : IUserRepository
{
    readonly List<UserData> data = [
        new("User 1", Guid.Parse("d57ffca5-26cf-4a51-b165-fdbaad3a296d")),
        new("User 2", Guid.Parse("cc224dab-4fe1-472c-aa1d-6b0846ad7712")),
    ];

    // assume API keys do not expire and there are only users with valid API keys
    public Task<User?> GetUserByApiKey(Guid apiKey)
    {
        var userData = data.FirstOrDefault(x => x.ApiKey.Equals(apiKey));
        User? user = userData != null ? new(userData.Name) : null;

        return Task.FromResult(user);
    }
}
