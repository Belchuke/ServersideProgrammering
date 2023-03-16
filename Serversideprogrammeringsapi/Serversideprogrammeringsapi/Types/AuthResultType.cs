using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.Env;
using Serversideprogrammeringsapi.ExtensionMethods;
using Serversideprogrammeringsapi.Repo.AESRepo;
using Serversideprogrammeringsapi.Repo.ToDoListRepo;

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

        public List<ToDoListType> ToDoLists([Service] ToDoDbContext toDoDbContext, [Service] IAESRepo _repo)
        {
            if (UserId == null)
            {
                return new List<ToDoListType>();
            }

            return toDoDbContext.ToDoLists
                .Where(x => x.UserId == UserId)
                .Select(list =>
                    new ToDoListType()
                    {
                        Id = list.Id,
                        Name = _repo.Decrypt(list.DataName, list.IVName),
                        Description = _repo.Decrypt(list.DataDescription, list.IVDescription),
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
