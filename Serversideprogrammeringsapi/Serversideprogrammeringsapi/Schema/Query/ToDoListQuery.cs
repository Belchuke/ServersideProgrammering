using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database;
using Serversideprogrammeringsapi.ExtensionMethods;
using Serversideprogrammeringsapi.Repo.AESRepo;
using Serversideprogrammeringsapi.Repo.ToDoListRepo;
using Serversideprogrammeringsapi.Types;
using System.Security.Claims;

namespace Serversideprogrammeringsapi.Schema.Query
{
    [ExtendObjectType(typeof(Query))]
    public class ToDoListQuery
    {
        [UseDbContext(typeof(ToDoDbContext))]
        [Authorize]
        [UseProjection]
        public async Task<ToDoListType> GetToDoListById([GraphQLName("toDoListId")] long id, ClaimsPrincipal claims, [Service] ToDoDbContext dbContext, [Service] IAESRepo _aesRepo)
        {
            ToDoListType get = await dbContext.ToDoLists
                .Where(x => x.Id == id)
                .Select(l =>
                    new ToDoListType()
                    {
                        IsSuccessful = true,
                        Message = "Ok",
                        Id = l.Id,
                        Name = _aesRepo.Decrypt(l.DataName, l.IVName),
                        Description = _aesRepo.Decrypt(l.DataDescription, l.IVDescription),
                        UserId = l.UserId,
                        Created = l.Created,
                        Disabled = l.Disabled,
                        IsEnabled = l.IsEnabled,
                        Updated = l.Updated
                    })
                .FirstAsync();

            if (!claims.IsAdmin() && get.UserId != claims.GetUserId())
            {
                return new ToDoListType()
                {
                    IsSuccessful = true,
                    Message = "user does not have permissions"
                };
            }

            return get;
        }

        [UseDbContext(typeof(ToDoDbContext))]
        [Authorize]
        [UseProjection]
        public async Task<ToDoListItemType> GetToDoListItemById([GraphQLName("toDoListItemId")] long id, ClaimsPrincipal claims, [Service] ToDoDbContext dbContext, [Service] IAESRepo _aesRepo, [Service] IToDoListRepo _toDoRepo)
        {
            ToDoListItemType get = await dbContext.ToDoListIteams
                .Where(x => x.Id == id)
                .Select(l =>
                    new ToDoListItemType()
                    {
                        IsSuccessful = true,
                        Message = "Ok",
                        Id = l.Id,
                        Name = _aesRepo.Decrypt(l.DataName, l.IVName),
                        Description = _aesRepo.Decrypt(l.DataDescription, l.IVDescription),
                        ToDoListId = l.ToDoListId,
                        Created = l.Created,
                        Disabled = l.Disabled,
                        IsEnabled = l.IsEnabled,
                        Updated = l.Updated
                    })
                .FirstAsync();

            if (!claims.IsAdmin() && await _toDoRepo.UserHasToDoListAsync(claims.GetUserId(), (long)get.ToDoListId))
            {
                return new ToDoListItemType()
                {
                    IsSuccessful = true,
                    Message = "user does not have permissions"
                };
            }

            return get;
        }

        [UseDbContext(typeof(ToDoDbContext))]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 50)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ToDoListType> GetUserToDoListsById([GraphQLName("userId")] long id, ClaimsPrincipal claims, [Service] ToDoDbContext dbContext, [Service] IAESRepo _aesRepo)
        {
            if (!claims.IsAdmin() && id != claims.GetUserId())
            {
                return Enumerable.Empty<ToDoListType>().AsQueryable();
            }

            return dbContext.ToDoLists
                .Where(x => x.UserId == id)
                .Select(l =>
                    new ToDoListType()
                    {
                        IsSuccessful = true,
                        Message = "Ok",
                        Id = l.Id,
                        Name = _aesRepo.Decrypt(l.DataName, l.IVName),
                        Description = _aesRepo.Decrypt(l.DataDescription, l.IVDescription),
                        UserId = l.UserId,
                        Created = l.Created,
                        Disabled = l.Disabled,
                        IsEnabled = l.IsEnabled,
                        Updated = l.Updated
                    });
        }
    }
}
