using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.Data.Dto;
using TaskManagement.Service.TaskServices;

namespace TaskManagement.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController(ITaskService taskService) : ControllerBase
    {
        private readonly ITaskService _taskService = taskService;

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<TaskResponseDto>>> GetTasks(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var isAdmin = User.IsInRole("Admin");

            var tasks = await _taskService.GetTasksAsync(userId, isAdmin, ct);
            return Ok(tasks);
        }

        // POST api/tasks
        [HttpPost("CreateTask")]
            public async Task<ActionResult<TaskResponseDto>> Create([FromBody] TaskCreateRequestDto dto, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var created = await _taskService.CreateAsync(dto, userId, ct);

            return Created($"/api/tasks/{created.Id}", created);
        }

        [HttpPut("UpdateTask{id:int}")]
        public async Task<ActionResult<TaskResponseDto>> Update(int id, [FromBody] TaskUpdateRequestDto dto, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            var isAdmin = User.IsInRole("Admin");

            var updated = await _taskService.UpdateAsync(id, dto, userId, isAdmin, ct);
            return Ok(updated);
        }

        // Admin only
        [HttpPatch("{id:int}/complete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TaskResponseDto>> MarkCompleted(int id, CancellationToken ct)
        {
            var completed = await _taskService.MarkCompletedAsync(id, ct);
            return Ok(completed);
        }
    }
}
