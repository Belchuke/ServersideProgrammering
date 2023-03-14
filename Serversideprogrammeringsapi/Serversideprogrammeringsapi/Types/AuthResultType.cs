using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.Env;
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

        public List<ToDoListType> ToDoLists([Service] ToDoDbContext toDoDbContext)
        {
            if (UserId == null)
            {
                return new List<ToDoListType>();
            }

            string aesKey = EnvHandler.GetAESKey();

            return toDoDbContext.ToDoLists
                .Where(x => x.UserId == UserId)
                .Select(list =>
                    new ToDoListType()
                    {
                        Id = list.Id,
                        Name = list.DataName.Decrypt(aesKey, list.IVName),
                        Created = DateTimeOffset.UtcNow,
                        Updated = DateTimeOffset.UtcNow,
                        Disabled = DateTimeOffset.UtcNow,
                        IsEnabled = true,
                        UserId = (long)UserId,
                    })
                .ToList();
        }
    }
}
