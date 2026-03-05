using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Data.Base
{
    public class BaseEntities
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }  // Primary Key

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        [Column("created_by")]
        public string? CreatedBy { get; set; } 

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; } = null;

        [MaxLength(100)]
        [Column("updated_by")]
        public string? UpdatedBy { get; set; } = null;

        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; } = null;

        [MaxLength(100)]
        [Column("deleted_by")]
        public string? DeletedBy { get; set; } = null;
    }
}
