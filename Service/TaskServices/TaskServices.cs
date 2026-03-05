using TaskManagement.Data.Dto;
using TaskManagement.Data.Entities;
using TaskManagement.Data.Repositories;

namespace TaskManagement.Service.TaskServices
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateAsync(TaskCreateRequestDto dto, string currentUserId, CancellationToken ct = default);
        Task<TaskResponseDto> UpdateAsync(int id, TaskUpdateRequestDto dto, string currentUserId, bool isAdmin, CancellationToken ct = default);
        Task<List<TaskResponseDto>> GetTasksAsync(string currentUserId, bool isAdmin, CancellationToken ct = default);
        Task<TaskResponseDto> MarkCompletedAsync(int id, CancellationToken ct = default); 

    }
    public class TaskService(ITaskRepository repo) : ITaskService
    {
        private readonly ITaskRepository _repo = repo;

        public async Task<TaskResponseDto> CreateAsync(TaskCreateRequestDto dto, string currentUserId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required.");

            var task = new TaskItem
            {
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                DueDate = dto.DueDate,
                OwnerUserId = currentUserId,
                CreatedAt = DateTime.UtcNow,
                IsCompleted = false
            };

            await _repo.AddAsync(task, ct);
            return Map(task);
        }

        public async Task<TaskResponseDto> UpdateAsync(int id, TaskUpdateRequestDto dto, string currentUserId, bool isAdmin, CancellationToken ct = default)
        {
            var task = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Task not found.");

            // Rule: users can update only their own tasks (admins can update any if you want; requirement says "Users can update their own tasks")
            if (!isAdmin && task.OwnerUserId != currentUserId)
                throw new UnauthorizedAccessException("You can only update your own tasks.");

            // Rule: users cannot update completion status (we simply don't expose it in DTO)
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required.");

            task.Title = dto.Title.Trim();
            task.Description = dto.Description?.Trim();
            task.DueDate = dto.DueDate;

            await _repo.UpdateAsync(task, ct);
            return Map(task);
        }

        public async Task<List<TaskResponseDto>> GetTasksAsync(string currentUserId, bool isAdmin, CancellationToken ct = default)
        {
            var tasks = isAdmin
                ? await _repo.GetAllAsync(ct)
                : await _repo.GetByOwnerAsync(currentUserId, ct);

            return tasks.Select(Map).ToList();
        }

        public async Task<TaskResponseDto> MarkCompletedAsync(int id, CancellationToken ct = default)
        {
            var task = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Task not found.");
            task.IsCompleted = true;

            await _repo.UpdateAsync(task, ct);
            return Map(task);
        }

        private static TaskResponseDto Map(TaskItem t) =>
            new(t.Id, t.Title, t.Description, t.IsCompleted, t.CreatedAt, t.DueDate, t.OwnerUserId);
    }
}
