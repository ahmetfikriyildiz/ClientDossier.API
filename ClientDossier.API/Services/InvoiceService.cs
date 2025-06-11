using ClientDossier.API.Data;
using ClientDossier.API.DTOs;
using ClientDossier.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientDossier.API.Services;

public class InvoiceService : IInvoiceService
{
    private readonly ApplicationDbContext _context;

    public InvoiceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InvoiceResponse>> GetInvoicesAsync(Guid clientId, Guid userId)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        return await _context.Invoices
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
    }

    public async Task<InvoiceResponse> GetInvoiceByIdAsync(Guid id, Guid userId)
    {
        var invoice = await _context.Invoices
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            throw new KeyNotFoundException("Invoice not found");

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == invoice.ClientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        return new InvoiceResponse
        {
            Id = invoice.Id,
            Amount = invoice.Amount,
            Currency = invoice.Currency,
            Description = invoice.Description,
            IssuedAt = invoice.IssuedAt,
            Paid = invoice.Paid,
            UserName = invoice.User.Name
        };
    }

    public async Task<InvoiceResponse> CreateInvoiceAsync(Guid clientId, CreateInvoiceRequest request, Guid userId)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        var invoice = new Invoice
        {
            ClientId = clientId,
            UserId = userId,
            Amount = request.Amount,
            Currency = request.Currency,
            Description = request.Description,
            IssuedAt = request.IssuedAt,
            Paid = false
        };

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);
        return new InvoiceResponse
        {
            Id = invoice.Id,
            Amount = invoice.Amount,
            Currency = invoice.Currency,
            Description = invoice.Description,
            IssuedAt = invoice.IssuedAt,
            Paid = invoice.Paid,
            UserName = user!.Name
        };
    }

    public async Task<InvoiceResponse> UpdateInvoiceAsync(Guid id, CreateInvoiceRequest request, Guid userId)
    {
        var invoice = await _context.Invoices
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            throw new KeyNotFoundException("Invoice not found");

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == invoice.ClientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        invoice.Amount = request.Amount;
        invoice.Currency = request.Currency;
        invoice.Description = request.Description;
        invoice.IssuedAt = request.IssuedAt;

        await _context.SaveChangesAsync();

        return new InvoiceResponse
        {
            Id = invoice.Id,
            Amount = invoice.Amount,
            Currency = invoice.Currency,
            Description = invoice.Description,
            IssuedAt = invoice.IssuedAt,
            Paid = invoice.Paid,
            UserName = invoice.User.Name
        };
    }

    public async Task DeleteInvoiceAsync(Guid id, Guid userId)
    {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.Id == id);

        if (invoice == null)
            throw new KeyNotFoundException("Invoice not found");

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == invoice.ClientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
    }
} 