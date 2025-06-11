namespace ClientDossier.API.Models;

public class TaskItem
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public ClientTaskStatus Status { get; set; } = ClientTaskStatus.TODO;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum ClientTaskStatus
{
    TODO,
    IN_PROGRESS,
    DONE
} 