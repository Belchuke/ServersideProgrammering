using HotChocolate.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.ExtensionMethods;
using Serversideprogrammeringsapi.Models;
using Serversideprogrammeringsapi.Types;
using System.Security.Claims;

namespace Serversideprogrammeringsapi.Schema.Query
{
    public class Query
    {
        [UseDbContext(typeof(ApiDbContext))]
        [Authorize]
        [UseProjection]
        public IQueryable<ToDoListType> GetUserLists(long userId, ApiDbContext context, ClaimsPrincipal claims)
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
