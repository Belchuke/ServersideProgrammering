using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.Env;
using Serversideprogrammeringsapi.ExtensionMethods;
using Serversideprogrammeringsapi.Repo.AESRepo;

namespace Serversideprogrammeringsapi.Types
{
    public class ToDoListType
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }

        [IsProjected(true)] // enforces requesting 
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }


        [IsProjected(true)] // enforces requesting 
        public long? UserId { get; set; }

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 50)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ToDoListItemType> ListItems([ScopedService] ToDoDbContext dbContext, [Service] IAESRepo _repo)
        {
            string aesKey = EnvHandler.GetAESKey();

            return dbContext.ToDoListIteams
                .Where(item => item.ToDoListId == Id)
                .Select(i => 
                    new ToDoListItemType()
                    {
                        Id = i.Id,
                        Name = _repo.Decrypt(i.DataName, i.IVName),
                        Description = _repo.Decrypt(i.DataDescription, i.IVDescription),
                        ToDoListId = (long)Id,
                        Created = i.Created,
                        Disabled = i.Disabled,
                        IsEnabled = i.IsEnabled,
                        Updated = i.Updated,
                    });
        }


        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }
        public bool? IsEnabled { get; set; } = true;
    }
}
