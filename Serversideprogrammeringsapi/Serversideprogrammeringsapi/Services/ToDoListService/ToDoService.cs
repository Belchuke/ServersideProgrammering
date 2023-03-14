using Serversideprogrammeringsapi.Database.Models;
using Serversideprogrammeringsapi.ExtensionMethods;
using Serversideprogrammeringsapi.Models;
using Serversideprogrammeringsapi.Repo.AESRepo;
using Serversideprogrammeringsapi.Repo.ToDoListRepo;
using Serversideprogrammeringsapi.Types;
using System.Security.Claims;

namespace Serversideprogrammeringsapi.Services.ToDoListService
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoListRepo _todoRepo;
        private readonly IAESRepo _aesRepo;

        public ToDoService(IToDoListRepo todoRepo, IAESRepo aesRepo)
        {
            _todoRepo = todoRepo;
            _aesRepo = aesRepo;
        }

        public async Task<ToDoListType> CreateToDoListAsync(CreateToDoListInput input, ClaimsPrincipal claims)
        {
            if (string.IsNullOrEmpty(input.Name))
            {
                return new ToDoListType()
                {
                    IsSuccessful = false,
                    Message = "Name required"
                };
            }

            if (string.IsNullOrEmpty(input.Description))
            {
                return new ToDoListType()
                {
                    IsSuccessful = false,
                    Message = "Description required"
                };
            }

            ToDoLists? result = await _todoRepo.CreateToDoListAsync(input, claims.GetUserId());

            if (result == null)
            {
                return new ToDoListType()
                {
                    IsSuccessful = false,
                    Message = "Insufient data passed"
                };
            }

            return new ToDoListType()
            {
                IsSuccessful = true,
                Message = "OK",
                Id = result.Id,
                Name = _aesRepo.Decrypt(result.DataName, result.IVName),
                Description = _aesRepo.Decrypt(result.DataDescription, result.IVDescription),
                Created = result.Created,
                Disabled = result.Disabled,
                IsEnabled = result.IsEnabled,
                Updated = result.Updated,
                UserId = result.UserId,
            };
        }

        public async Task<ToDoListItemType> CreateToDoListItemAsync(CreateToDoListItemInput input, ClaimsPrincipal claims)
        {
            if (string.IsNullOrEmpty(input.Name))
            {
                return new ToDoListItemType()
                {
                    IsSuccessful = false,
                    Message = "Name required"
                };
            }

            if (string.IsNullOrEmpty(input.Description))
            {
                return new ToDoListItemType()
                {
                    IsSuccessful = false,
                    Message = "Description required"
                };
            }

            if (input.ToDoListId == null && input.ToDoListId == 0)
            {
                return new ToDoListItemType()
                {
                    IsSuccessful = false,
                    Message = "ToDoListId required"
                };
            }

            if (!await _todoRepo.UserHasToDoListAsync(claims.GetUserId(), (long)input.ToDoListId))
            {
                return new ToDoListItemType()
                {
                    IsSuccessful = false,
                    Message = "user does not have permissions"
                };
            }


            ToDoListIteam? result = await _todoRepo.CreateToDoListItemAsync(input);

            if (result == null)
            {
                return new ToDoListItemType()
                {
                    IsSuccessful = false,
                    Message = "Insufient data passed"
                };
            }

            return new ToDoListItemType()
            {
                IsSuccessful = true,
                Message = "OK",
                Id = result.Id,
                Name = _aesRepo.Decrypt(result.DataName, result.IVName),
                Description = _aesRepo.Decrypt(result.DataDescription, result.IVDescription),
                ToDoListId = result.ToDoListId,
                Created = result.Created,
                Disabled = result.Disabled,
                IsEnabled = result.IsEnabled,
                Updated = result.Updated,
            };
        }

        public async Task<ToDoListType> UpdateToDoListAsync(UpdateToDoListInput input, ClaimsPrincipal claims)
        {
            ToDoLists get = await _todoRepo.GetToDoListByIdAsync(input.Id);

            if (get.UserId != claims.GetUserId())
            {
                return new ToDoListType()
                {
                    IsSuccessful = false,
                    Message = "user does not have permissions"
                };
            }

            if (input.Delete != null && (bool)input.Delete)
            {
                await _todoRepo.DeleteToDoListAsync(get);
                return new ToDoListType()
                {
                    IsSuccessful = true,
                    Message = "Deleted"
                };
            }
            else
            {
                string currentName = _aesRepo.Decrypt(get.DataName, get.IVName);
                string currentDescription = _aesRepo.Decrypt(get.DataDescription, get.IVDescription);

                if (input.Items != null && input.Items.Count > 0)
                {
                    await _todoRepo.UpdateToDoListsItemsAsync(get, input);
                }

                if (input.Name != null && input.Name != currentName)
                {
                    AESEncryptResult encryptName = _aesRepo.Encrypt(input.Name);

                    get.DataName = encryptName.EncryptedText;
                    get.IVName = encryptName.IV;
                }

                if (input.Description != null && input.Description != currentDescription)
                {
                    AESEncryptResult encryptDescription = _aesRepo.Encrypt(input.Description);

                    get.DataDescription = encryptDescription.EncryptedText;
                    get.IVDescription = encryptDescription.IV;
                }

                if (input.IsEnabled != null && (bool)input.IsEnabled != get.IsEnabled)
                {
                    get.IsEnabled = (bool)input.IsEnabled;
                }

                ToDoLists updated = await _todoRepo.UpdateToDoListAsync(get);

                return new ToDoListType()
                {
                    IsSuccessful = true,
                    Message = "Updated",
                    Id = updated.Id,
                    Name = _aesRepo.Decrypt(updated.DataName, updated.IVName),
                    Description = _aesRepo.Decrypt(updated.DataDescription, updated.IVDescription),
                    Created = updated.Created,
                    Disabled = updated.Disabled,
                    IsEnabled = updated.IsEnabled,
                    Updated = updated.Updated,
                    UserId = updated.UserId,
                };
            }
        }

        public async Task<ToDoListItemType> UpdateToDoListItemAsync(UpdateToDoListItemInput input, ClaimsPrincipal claims)
        {
            if (input.Id == null && input.Id == 0)
            {
                return new ToDoListItemType()
                {
                    IsSuccessful = false,
                    Message = "Id Required for update"
                };
            }

            ToDoListIteam get = await _todoRepo.GetToDoListItemByIdAsync((long)input.Id);

            if (!await _todoRepo.UserHasToDoListBasedOnItemAsync(claims.GetUserId(), get))
            {
                return new ToDoListItemType()
                {
                    IsSuccessful = false,
                    Message = "user does not have permissions"
                };
            }

            if (input.Delete != null && (bool)input.Delete)
            {
                await _todoRepo.DeleteToDoListItemAsync(get);
                return new ToDoListItemType()
                {
                    IsSuccessful = true,
                    Message = "Deleted"
                };
            }
            else
            {
                string currentName = _aesRepo.Decrypt(get.DataName, get.IVName);
                string currentDescription = _aesRepo.Decrypt(get.DataDescription, get.IVDescription);

                if (input.Name != null && input.Name != currentName)
                {
                    AESEncryptResult encryptName = _aesRepo.Encrypt(input.Name);

                    get.DataName = encryptName.EncryptedText;
                    get.IVName = encryptName.IV;
                }

                if (input.Description != null && input.Description != currentDescription)
                {
                    AESEncryptResult encryptDescription = _aesRepo.Encrypt(input.Description);

                    get.DataDescription = encryptDescription.EncryptedText;
                    get.IVDescription = encryptDescription.IV;
                }

                if (input.IsEnabled != null && (bool)input.IsEnabled != get.IsEnabled)
                {
                    get.IsEnabled = (bool)input.IsEnabled;
                }

                ToDoListIteam updated = await _todoRepo.UpdateToDoListItemAsync(get);

                return new ToDoListItemType()
                {
                    IsSuccessful = true,
                    Message = "Updated",
                    Id = updated.Id,
                    Name = _aesRepo.Decrypt(updated.DataName, updated.IVName),
                    Description = _aesRepo.Decrypt(updated.DataDescription, updated.IVDescription),
                    ToDoListId = updated.ToDoListId,
                    Created = updated.Created,
                    Disabled = updated.Disabled,
                    IsEnabled = updated.IsEnabled,
                    Updated = updated.Updated,
                };
            }
        }
    }
}
