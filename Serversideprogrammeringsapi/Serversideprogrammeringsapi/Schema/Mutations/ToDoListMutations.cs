using HotChocolate.Authorization;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.Models;
using Serversideprogrammeringsapi.Services.ToDoListService;
using Serversideprogrammeringsapi.Types;
using System.Security.Claims;

namespace Serversideprogrammeringsapi.Schema.Mutations
{
    [ExtendObjectType(typeof(Mutation))]
    public class ToDoListMutations
    {
        [UseDbContext(typeof(ToDoDbContext))]
        [Authorize]
        [UseProjection]
        public async Task<ToDoListType> CreateToDoList(CreateToDoListInput input, ClaimsPrincipal claims, [Service] IToDoService service)
        {
            return await service.CreateToDoListAsync(input, claims);
        }

        [UseDbContext(typeof(ToDoDbContext))]
        [Authorize]
        [UseProjection]
        public async Task<ToDoListItemType> CreateToDoListItem(CreateToDoListItemInput input, ClaimsPrincipal claims, [Service] IToDoService service)
        {
            return await service.CreateToDoListItemAsync(input, claims);
        }

        [UseDbContext(typeof(ToDoDbContext))]
        [Authorize]
        [UseProjection]
        public async Task<ToDoListType> UpdateToDoList(UpdateToDoListInput input, ClaimsPrincipal claims, [Service] IToDoService service)
        {
            return await service.UpdateToDoListAsync(input, claims);
        }

        [UseDbContext(typeof(ToDoDbContext))]
        [Authorize]
        [UseProjection]
        public async Task<ToDoListItemType> UpdateToDoListItem(UpdateToDoListItemInput input, ClaimsPrincipal claims, [Service] IToDoService service)
        {
            return await service.UpdateToDoListItemAsync(input, claims);
        }
    }
}
