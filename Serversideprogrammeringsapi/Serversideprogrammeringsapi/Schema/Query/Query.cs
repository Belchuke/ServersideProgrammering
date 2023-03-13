using HotChocolate.Authorization;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.ExtensionMethods;
using Serversideprogrammeringsapi.Types;
using System.Security.Claims;

namespace Serversideprogrammeringsapi.Schema.Query
{
    public class Query
    {
        [UseDbContext(typeof(ApiDbContext))]
        [Authorize]
        [UseProjection]
        public IQueryable<ToDoListType> GetUserLists(long userId, [ScopedService] ApiDbContext context, ClaimsPrincipal claims)
        {
            if (!claims.IsAdmin())
            {
                if (claims.GetUserId() != userId)
                {
                    throw new BadHttpRequestException("Permissions missing");
                }
            }

            return null;
        }
    }
}
