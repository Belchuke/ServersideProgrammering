using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.Database.Models;

namespace Serversideprogrammeringsapi.Identity.Repo
{
    public class RefreshTokenRepo : IRefreshTokenRepo
    {
        private readonly IDbContextFactory<ApiDbContext> _dbContextFactory;

        public RefreshTokenRepo(IDbContextFactory<ApiDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<AuthRefreshToken> GetByGuidAsync(Guid id)
        {
            await using ApiDbContext dbContext = _dbContextFactory.CreateDbContext();

           return await dbContext.AuthRefreshToken
                .FirstOrDefaultAsync(token => token.Id == id);
        }

        public async Task<AuthRefreshToken> GetByUserIdAsync(long id)
        {
            await using ApiDbContext dbContext = _dbContextFactory.CreateDbContext();


            return await dbContext.AuthRefreshToken
                 .FirstOrDefaultAsync(token => token.UserId == id);
        }

        public async Task AddAsync(AuthRefreshToken token)
        {
            await using ApiDbContext dbContext = _dbContextFactory.CreateDbContext();

            await dbContext.AuthRefreshToken.AddAsync(token);
            await dbContext.SaveChangesAsync();
        }

    }
}
