using System.ComponentModel.DataAnnotations;
using ClientDossier.API.Models;

namespace ClientDossier.API.DTOs;

public class CreateTaskRequest
{
    [Required]
    public string Title { get; set; } = null!;
    
    public string? Description { get; set; }
    public ClientTaskStatus Status { get; set; } = ClientTaskStatus.TODO;
    public DateTime? DueDate { get; set; }
}

public class TaskResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public ClientTaskStatus Status { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserName { get; set; } = null!;
} 