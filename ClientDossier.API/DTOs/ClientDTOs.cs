using System.ComponentModel.DataAnnotations;

namespace ClientDossier.API.DTOs;

public class CreateClientRequest
{
    [Required]
    public string Name { get; set; } = null!;
    
    [EmailAddress]
    public string? Email { get; set; }
    
    [Phone]
    public string? Phone { get; set; }
    
    public string? CompanyName { get; set; }
    public string? Notes { get; set; }
}

public class UpdateClientRequest
{
    [Required]
    public string Name { get; set; } = null!;
    
    [EmailAddress]
    public string? Email { get; set; }
    
    [Phone]
    public string? Phone { get; set; }
    
    public string? CompanyName { get; set; }
    public string? Notes { get; set; }
}

public class ClientResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
} 