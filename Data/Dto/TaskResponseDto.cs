namespace TaskManagement.Data.Dto
{
    public record TaskCreateRequestDto(string Title, string? Description, DateTime? DueDate);
    public record TaskUpdateRequestDto(string Title, string? Description, DateTime? DueDate);

    public record TaskResponseDto(
        int Id,
        string Title,
        string? Description,
        bool IsCompleted,
        DateTime CreatedAt,
        DateTime? DueDate,
        string OwnerUserId
    );
}
