using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database.Models;

namespace Serversideprogrammeringsapi.Database
{
    public class ApiDbContext : IdentityDbContext<ApiUser, ApiRole, long, IdentityUserClaim<long>, ApiUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public virtual DbSet<AuthRefreshToken> AuthRefreshToken { get; set; }
        public virtual DbSet<SignupOtp> SignupOtps { get; set; }
        public virtual DbSet<UserToDoLists> UserToDoLists { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
            SavingChanges += ApiDbContext_SavingChanges;
        }

        private void ApiDbContext_SavingChanges(object? sender, SavingChangesEventArgs? e)
        {
            UpdateCreatedChangedDisabled();
        }

        public void UpdateCreatedChangedDisabled()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;

            foreach (var entry in ChangeTracker.Entries<IDatedEntity>())
            {
                var entity = entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.Created = now;
                        entity.Updated = now;
                        break;

                    case EntityState.Modified:
                        entity.Updated = now;
                        if (!entity.IsEnabled)
                            entity.Disabled = now;
                        break;
                }
            }

            ChangeTracker.DetectChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserToDoLists>(entity =>
            {
                entity.HasKey(k => new
                {
                    k.UserId,
                    k.ToDoListId
                });
            });
        }
    }
}
