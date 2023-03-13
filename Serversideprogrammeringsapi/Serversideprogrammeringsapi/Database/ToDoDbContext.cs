using Microsoft.EntityFrameworkCore;
using Serversideprogrammeringsapi.Database.Models;

namespace Serversideprogrammeringsapi.Database
{
    public class ToDoDbContext : DbContext
    {
        public DbSet<ToDoLists> ToDoLists { get; set; }
        public DbSet<ToDoListIteam> ToDoListIteams { get; set; }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
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
        }
    }
}
