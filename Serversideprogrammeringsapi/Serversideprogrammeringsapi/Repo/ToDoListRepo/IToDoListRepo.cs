using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.Models;

namespace Serversideprogrammeringsapi.Repo.ToDoListRepo
{
    public interface IToDoListRepo
    {
        Task<ToDoLists> GetToDoListByIdAsync(long id);

        Task<ToDoListIteam> GetToDoListItemByIdAsync(long id);

        Task<bool> UserHasToDoListAsync(long userId, long listId);

        Task<bool> UserHasToDoListBasedOnItemAsync(long userId, ToDoListIteam item);

        Task<ToDoLists> CreateToDoListAsync(CreateToDoListInput input, long userId);

        Task<ToDoListIteam> CreateToDoListItemAsync(CreateToDoListItemInput input);

        Task<ToDoLists> UpdateToDoListAsync(ToDoLists list);

        Task UpdateToDoListsItemsAsync(ToDoLists list, UpdateToDoListInput input);

        Task<ToDoListIteam> UpdateToDoListItemAsync(ToDoListIteam item);

        Task DeleteToDoListAsync(ToDoLists list);

        Task DeleteToDoListItemAsync(ToDoListIteam item);
    }
}
