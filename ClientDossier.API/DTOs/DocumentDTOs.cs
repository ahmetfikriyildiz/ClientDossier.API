using System.ComponentModel.DataAnnotations;

namespace ClientDossier.API.DTOs;

public class CreateDocumentRequest
{
    [Required]
    public string FileName { get; set; } = null!;
    
    [Required]
    public IFormFile File { get; set; } = null!;
}

public class DocumentResponse
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public DateTime UploadedAt { get; set; }
} 