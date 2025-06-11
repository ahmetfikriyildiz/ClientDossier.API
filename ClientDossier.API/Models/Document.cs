namespace ClientDossier.API.Models;

public class Document
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public string FileName { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
} 