using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.ExtensionMethods;

namespace Serversideprogrammeringsapi.Types
{
    public class AuthResultType
    {
        public bool IsSuccessful { get; set; }

        public string? Message { get; set; }

        public string? Token { get; set; }
        public DateTimeOffset? Expires { get; set; }
        public string? RefreshToken { get; set; }

        [IsProjected(true)]
        public long? UserId { get; set; }

        public UserType? User { get; set; }

        public async Task<List<ToDoListType>> ToDoLists([ScopedService] ToDoDbContext toDoDbContext)
        {
            if (UserId == null)
            {
                return new List<ToDoListType>();
            }

            return await toDoDbContext.ToDoLists
                .Where(x => x.UserId == UserId)
                .Select(list =>
                    new ToDoListType()
                    {
                        Id = list.Id,
                        Name = list.DataName.Decrypt(list.KeyName, list.IVName),
                        Created = DateTimeOffset.UtcNow,
                        Updated = DateTimeOffset.UtcNow,
                        Disabled = DateTimeOffset.UtcNow,
                        IsEnabled = true,
                        UserId = (long)UserId,
                    })
                .ToListAsync();
        }
    }
}
