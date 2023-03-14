using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.Env;
using Serversideprogrammeringsapi.ExtensionMethods;

namespace Serversideprogrammeringsapi.Types
{
    public class ToDoListType
    {
        [IsProjected(true)] // enforces requesting 
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UserId { get; set; }

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 50)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ToDoListItemType> ListItems([ScopedService] ToDoDbContext dbContext)
        {
            string aesKey = EnvHandler.GetAESKey();

            return dbContext.ToDoListIteams
                .Where(item => item.ToDoListId == Id)
                .Select(i => 
                    new ToDoListItemType()
                    {
                        Id = i.Id,
                        Name = i.DataName.Decrypt(aesKey, i.IVName),
                        Description = i.DataDescription.Decrypt(aesKey, i.IVDescription),
                        ToDoListId = Id,
                        Created = i.Created,
                        Disabled = i.Disabled,
                        IsEnabled = i.IsEnabled,
                        Updated = i.Updated,
                    });
        }


        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
