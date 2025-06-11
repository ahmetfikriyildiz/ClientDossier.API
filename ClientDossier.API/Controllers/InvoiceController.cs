using ClientDossier.API.Data;
using ClientDossier.API.DTOs;
using ClientDossier.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ClientDossier.API.Controllers;

[Authorize]
[ApiController]
[Route("api/clients/{clientId}/invoices")]
public class InvoiceController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InvoiceController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InvoiceResponse>>> GetInvoices(Guid clientId)
    {
        var userId = GetCurrentUserId();
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
        {
            return NotFound("Client not found");
        }

        var invoices = await _context.Invoices
            .Where(i => i.ClientId == clientId)
            .Select(i => new InvoiceResponse
            {
                Id = i.Id,
                Amount = i.Amount,
                Currency = i.Currency,
                Description = i.Description,
                IssuedAt = i.IssuedAt,
                Paid = i.Paid,
                UserName = i.User.Name
            })
            .ToListAsync();

        return Ok(invoices);
    }

    [HttpPost]
    public async Task<ActionResult<InvoiceResponse>> CreateInvoice(Guid clientId, CreateInvoiceRequest request)
    {
        var userId = GetCurrentUserId();
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
        {
            return NotFound("Client not found");
        }

        var invoice = new Invoice
        {
            ClientId = clientId,
            UserId = userId,
            Amount = request.Amount,
            Currency = request.Currency,
            Description = request.Description,
            IssuedAt = request.IssuedAt
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);
        return Ok(new InvoiceResponse
        {
            Id = invoice.Id,
            Amount = invoice.Amount,
            Currency = invoice.Currency,
            Description = invoice.Description,
            IssuedAt = invoice.IssuedAt,
            Paid = invoice.Paid,
            UserName = user!.Name
        });
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new InvalidOperationException("User ID not found in claims");
        }
        return Guid.Parse(userIdClaim.Value);
    }
} 