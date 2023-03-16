using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.Env;
using Serversideprogrammeringsapi.ExtensionMethods;
using Serversideprogrammeringsapi.Repo.AESRepo;

namespace Serversideprogrammeringsapi.Types
{
    public class ToDoListItemType
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }

        [IsProjected(true)]
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [IsProjected(true)]
        public long? ToDoListId { get; set; }

        [UseProjection]
        public async Task<ToDoListType> ToDoList([ScopedService] ToDoDbContext dbContext, [Service] IAESRepo _repo)
        {
            string aesKey = EnvHandler.GetAESKey();

            return await dbContext.ToDoLists
                .Where(x => x.Id == ToDoListId)
                .Select(list => 
                    new ToDoListType()
                    {
                        Id = list.Id,
                        UserId = list.UserId,
                        Name = _repo.Decrypt(list.DataName, list.IVName),
                        Description = _repo.Decrypt(list.DataDescription, list.IVDescription),
                        Created = list.Created,
                        Updated = list.Updated,
                        Disabled = list.Disabled,
                        IsEnabled = list.IsEnabled,
                    })
                .FirstAsync();
        }

        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }
        public bool? IsEnabled { get; set; } = true;
    }
}
