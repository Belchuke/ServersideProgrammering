using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.Models;
using Serversideprogrammeringsapi.Repo.AESRepo;

namespace Serversideprogrammeringsapi.Repo.ToDoListRepo
{
    public class ToDoListRepo : IToDoListRepo
    {
        private readonly IDbContextFactory<ToDoDbContext> _contextFactoryToDo;
        private readonly IAESRepo _aesRepo;

        public ToDoListRepo(IDbContextFactory<ToDoDbContext> contextFactoryToDo, IAESRepo aesRepo)
        {
            _contextFactoryToDo = contextFactoryToDo;
            _aesRepo = aesRepo;
        }

        public async Task<ToDoLists> GetToDoListByIdAsync(long id)
        {
            using ToDoDbContext context = _contextFactoryToDo.CreateDbContext();

            return await context.ToDoLists.FirstAsync(x => x.Id == id);
        }

        public async Task<ToDoListIteam> GetToDoListItemByIdAsync(long id)
        {
            using ToDoDbContext context = _contextFactoryToDo.CreateDbContext();

            return await context.ToDoListIteams.FirstAsync(x => x.Id == id);
        }

        public async Task<bool> UserHasToDoListAsync(long userId, long listId)
        {
            using ToDoDbContext context = _contextFactoryToDo.CreateDbContext();

            return await context.ToDoLists.AnyAsync(x => x.Id == listId && x.UserId == userId);
        }

        public async Task<bool> UserHasToDoListBasedOnItemAsync(long userId, ToDoListIteam item)
        {
            using ToDoDbContext context = _contextFactoryToDo.CreateDbContext();

            return await context.ToDoLists.AnyAsync(x => x.UserId == userId && x.Id == item.ToDoListId);
        }

        public async Task<ToDoLists> CreateToDoListAsync(CreateToDoListInput input, long userId)
        {
            using ToDoDbContext toDoContext = _contextFactoryToDo.CreateDbContext();

            AESEncryptResult encryptName = _aesRepo.Encrypt(input.Name);

            AESEncryptResult encryptDescription = _aesRepo.Encrypt(input.Description);

            ToDoLists newList = new ToDoLists()
            {
                UserId = userId,
                DataName = encryptName.EncryptedText,
                IVName = encryptName.IV,
                DataDescription = encryptDescription.EncryptedText,
                IVDescription = encryptDescription.IV,
                ListItems = new List<ToDoListIteam>(),
            };

            if (input.Items != null && input.Items.Count > 0)
            {
                foreach (CreateToDoListItemInput item in input.Items)
                {
                    AESEncryptResult encryptListName = _aesRepo.Encrypt(item.Name);

                    AESEncryptResult encryptListDescription = _aesRepo.Encrypt(item.Description);

                    newList.ListItems.Add(new ToDoListIteam()
                    {
                        DataName = encryptListName.EncryptedText,
                        IVName = encryptListName.IV,
                        DataDescription = encryptListDescription.EncryptedText,
                        IVDescription = encryptListDescription.IV,
                    });
                }
            }

            await toDoContext.ToDoLists.AddAsync(newList);
            await toDoContext.SaveChangesAsync();

            return newList;
        }

        public async Task<ToDoListIteam> CreateToDoListItemAsync(CreateToDoListItemInput input)
        {
            using ToDoDbContext toDoContext = _contextFactoryToDo.CreateDbContext();

            AESEncryptResult encryptName = _aesRepo.Encrypt(input.Name);

            AESEncryptResult encryptDescription = _aesRepo.Encrypt(input.Description);

            ToDoListIteam newItem = new ToDoListIteam()
            {
                DataName = encryptName.EncryptedText,
                IVName = encryptName.IV,
                DataDescription = encryptDescription.EncryptedText,
                IVDescription = encryptDescription.IV,
                ToDoListId = (long)input.ToDoListId,
            };

            await toDoContext.ToDoListIteams.AddAsync(newItem);
            await toDoContext.SaveChangesAsync();

            return newItem;
        }

        public async Task<ToDoLists> UpdateToDoListAsync(ToDoLists list)
        {
            using ToDoDbContext context = _contextFactoryToDo.CreateDbContext();

            context.ToDoLists.Update(list);
            await context.SaveChangesAsync();

            return await context.ToDoLists.FirstAsync(x => x.Id == list.Id);
        }

        public async Task UpdateToDoListsItemsAsync(ToDoLists list, UpdateToDoListInput input)
        {
            using ToDoDbContext context = _contextFactoryToDo.CreateDbContext();

            List<long> getIds = input.Items.Where(x => x.Id != null).Select(i => (long)i.Id).ToList();

            List<ToDoListIteam> thisListCurrentItems = await context.ToDoListIteams
                .Where(item => getIds.Contains(item.Id))
                .ToListAsync();

            foreach (UpdateToDoListItemInput item in input.Items)
            {
                ToDoListIteam? getItem = thisListCurrentItems.FirstOrDefault(x => x.Id == item.Id);

                if(getItem != null)
                {
                    if (item.Delete != null && (bool)item.Delete)
                    {
                        thisListCurrentItems.Remove(getItem);
                        context.ToDoListIteams.Remove(getItem);
                    }
                    else
                    {
                        string currentName = _aesRepo.Decrypt(getItem.DataName, getItem.IVName);
                        string currentDescription = _aesRepo.Decrypt(getItem.DataDescription, getItem.IVDescription);

                        if (item.Name != null && item.Name != currentName)
                        {
                            AESEncryptResult encryptName = _aesRepo.Encrypt(item.Name);

                            getItem.DataName = encryptName.EncryptedText;
                            getItem.IVName = encryptName.IV;
                        }

                        if (item.Description != null && item.Description != currentDescription)
                        {
                            AESEncryptResult encryptDescription = _aesRepo.Encrypt(item.Description);

                            getItem.DataDescription = encryptDescription.EncryptedText;
                            getItem.IVDescription = encryptDescription.IV;
                        }

                        if (item.IsEnabled != null && (bool)item.IsEnabled != getItem.IsEnabled)
                        {
                            getItem.IsEnabled = (bool)item.IsEnabled;
                        }
                    }
                }
                else
                {
                    AESEncryptResult encryptName = _aesRepo.Encrypt(item.Name);

                    AESEncryptResult encryptDescription = _aesRepo.Encrypt(item.Description);

                    thisListCurrentItems.Add(new ToDoListIteam()
                    {
                        DataName = encryptName.EncryptedText,
                        IVName = encryptName.IV,
                        DataDescription = encryptDescription.EncryptedText,
                        IVDescription = encryptDescription.IV,
                        ToDoListId = list.Id,
                    });
                }
            }

            context.ToDoListIteams.UpdateRange(thisListCurrentItems);
            await context.SaveChangesAsync();
        }

        public async Task<ToDoListIteam> UpdateToDoListItemAsync(ToDoListIteam item)
        {
            using ToDoDbContext context = _contextFactoryToDo.CreateDbContext();

            context.ToDoListIteams.Update(item);
            await context.SaveChangesAsync();

            return await context.ToDoListIteams.FirstAsync(x => x.Id == item.Id);
        }

        public async Task DeleteToDoListAsync(ToDoLists list)
        {
            using ToDoDbContext context = _contextFactoryToDo.CreateDbContext();

            context.ToDoLists.Remove(list);
            await context.SaveChangesAsync();
        }

        public async Task DeleteToDoListItemAsync(ToDoListIteam item)
        {
            using ToDoDbContext context = _contextFactoryToDo.CreateDbContext();

            context.ToDoListIteams.Remove(item);
            await context.SaveChangesAsync();
        }
    }
}
