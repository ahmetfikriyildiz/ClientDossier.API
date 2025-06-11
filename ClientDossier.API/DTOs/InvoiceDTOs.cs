using System.ComponentModel.DataAnnotations;

namespace ClientDossier.API.DTOs;

public class CreateInvoiceRequest
{
    [Required]
    [Range(0.01, float.MaxValue)]
    public float Amount { get; set; }
    
    [Required]
    [StringLength(3)]
    public string Currency { get; set; } = "USD";
    
    public string? Description { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
}

public class InvoiceResponse
{
    public Guid Id { get; set; }
    public float Amount { get; set; }
    public string Currency { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime IssuedAt { get; set; }
    public bool Paid { get; set; }
    public string UserName { get; set; } = null!;
} 