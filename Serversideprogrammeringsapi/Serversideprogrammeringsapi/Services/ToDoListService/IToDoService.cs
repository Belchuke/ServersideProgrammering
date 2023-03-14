using Serversideprogrammeringsapi.Models;
using Serversideprogrammeringsapi.Types;
using System.Security.Claims;

namespace Serversideprogrammeringsapi.Services.ToDoListService
{
    public interface IToDoService
    {

        Task<ToDoListType> CreateToDoListAsync(CreateToDoListInput input, ClaimsPrincipal claims);

        Task<ToDoListItemType> CreateToDoListItemAsync(CreateToDoListItemInput input, ClaimsPrincipal claims);

        Task<ToDoListType> UpdateToDoListAsync(UpdateToDoListInput input, ClaimsPrincipal claims);

        Task<ToDoListItemType> UpdateToDoListItemAsync(UpdateToDoListItemInput input, ClaimsPrincipal claims);
    }
}
