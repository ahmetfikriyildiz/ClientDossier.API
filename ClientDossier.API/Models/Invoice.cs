namespace ClientDossier.API.Models;

public class Invoice
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public float Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string? Description { get; set; }
    public DateTime IssuedAt { get; set; }
    public bool Paid { get; set; } = false;
} 