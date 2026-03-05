using Microsoft.EntityFrameworkCore;
using TaskManagement.Data.Entities;
using TaskManagement.Utils;

namespace TaskManagement.Data.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskItem> AddAsync(TaskItem task, CancellationToken ct = default);
        Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default);
        Task<List<TaskItem>> GetByOwnerAsync(string ownerUserId, CancellationToken ct = default);
        Task UpdateAsync(TaskItem task, CancellationToken ct = default);
    }
    public class TaskRepository(AppDbContext db) : ITaskRepository
    {
        private readonly AppDbContext _db = db;

        public async Task<TaskItem> AddAsync(TaskItem task, CancellationToken ct = default)
        {
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync(ct);
            return task;
        }

        public Task<TaskItem?> GetByIdAsync(int id, CancellationToken ct = default)
            => _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);

        public Task<List<TaskItem>> GetAllAsync(CancellationToken ct = default)
            => _db.Tasks.OrderByDescending(t => t.CreatedAt).ToListAsync(ct);

        public Task<List<TaskItem>> GetByOwnerAsync(string ownerUserId, CancellationToken ct = default)
            => _db.Tasks
                .Where(t => t.OwnerUserId == ownerUserId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(ct);

        public async Task UpdateAsync(TaskItem task, CancellationToken ct = default)
        {
            _db.Tasks.Update(task);
            await _db.SaveChangesAsync(ct);
        }
    }
    }
