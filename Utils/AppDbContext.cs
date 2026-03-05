using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using TaskManagement.Data.Entities;

namespace TaskManagement.Utils
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional extras (indexes/defaults)
            modelBuilder.Entity<TaskItem>()
                .HasIndex(x => x.OwnerUserId);

            modelBuilder.Entity<TaskItem>()
                .Property(x => x.IsCompleted)
                .HasDefaultValue(false);
        }
    }
}
