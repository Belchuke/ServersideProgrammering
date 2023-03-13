using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.ExtensionMethods;

namespace Serversideprogrammeringsapi.Types
{
    public class AuthResultType
    {
        public string? Token { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public string? RefreshToken { get; set; }

        [IsProjected(true)]
        public long UserId { get; set; }

        public IQueryable<ToDoListType> ToDoLists([ScopedService] ToDoDbContext toDoDbContext)
        {
            return toDoDbContext.ToDoLists
                .Where(x => x.UserId == UserId)
                .Select(list =>
                    new ToDoListType()
                    {
                        Id = list.Id,
                        Name = list.Data.Decrypt(list.Key, list.IV),
                        Created = DateTimeOffset.UtcNow,
                        Updated = DateTimeOffset.UtcNow,
                        Disabled = DateTimeOffset.UtcNow,
                        IsEnabled = true,
                        UserId = UserId,
                    });
        }
    }
}
