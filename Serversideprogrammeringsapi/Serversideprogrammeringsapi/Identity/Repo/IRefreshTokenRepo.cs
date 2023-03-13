using Serversideprogrammeringsapi.Database.Models;

namespace Serversideprogrammeringsapi.Identity.Repo
{
    public interface IRefreshTokenRepo
    {
        Task<AuthRefreshToken> GetByUserIdAsync(long id);

        Task<AuthRefreshToken> GetByGuidAsync(Guid id);
        Task AddAsync(AuthRefreshToken token);
    }
}
