using System.ComponentModel.DataAnnotations.Schema;
using TaskManagement.Data.Base;

namespace TaskManagement.Data.Entities
{
    [Table("Tasks")]
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public string OwnerUserId { get; set; } = null!;
    }
}
