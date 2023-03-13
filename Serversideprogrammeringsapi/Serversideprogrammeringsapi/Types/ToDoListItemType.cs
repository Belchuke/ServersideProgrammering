using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.ExtensionMethods;

namespace Serversideprogrammeringsapi.Types
{
    public class ToDoListItemType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [IsProjected(true)]
        public long ToDoListId { get; set; }

        [UseProjection]
        public async Task<ToDoListType> ToDoList([ScopedService] ToDoDbContext dbContext)
        {
            return await dbContext.ToDoLists
                .Where(x => x.Id == ToDoListId)
                .Select(list => 
                    new ToDoListType()
                    {
                        Id = list.Id,
                        UserId = list.UserId,
                        Name = list.DataName.Decrypt(list.KeyName, list.IVName),
                        Description = list.DataDescription.Decrypt(list.KeyDescription, list.IVDescription),
                    })
                .FirstAsync();
        }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset? Disabled { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
