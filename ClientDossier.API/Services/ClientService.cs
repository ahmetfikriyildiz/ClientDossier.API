using ClientDossier.API.Data;
using ClientDossier.API.DTOs;
using ClientDossier.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientDossier.API.Services;

public class ClientService : IClientService
{
    private readonly ApplicationDbContext _context;

    public ClientService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClientResponse>> GetClientsAsync(Guid userId)
    {
        return await _context.Clients
            .Where(c => c.UserId == userId)
            .Select(c => new ClientResponse
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Phone = c.Phone,
                CompanyName = c.CompanyName,
                Notes = c.Notes,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ClientResponse> GetClientByIdAsync(Guid id, Guid userId)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        return new ClientResponse
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            CompanyName = client.CompanyName,
            Notes = client.Notes,
            CreatedAt = client.CreatedAt
        };
    }

    public async Task<ClientResponse> CreateClientAsync(CreateClientRequest request, Guid userId)
    {
        var client = new Client
        {
            UserId = userId,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            CompanyName = request.CompanyName,
            Notes = request.Notes
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return new ClientResponse
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            CompanyName = client.CompanyName,
            Notes = client.Notes,
            CreatedAt = client.CreatedAt
        };
    }

    public async Task<ClientResponse> UpdateClientAsync(Guid id, UpdateClientRequest request, Guid userId)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        client.Name = request.Name;
        client.Email = request.Email;
        client.Phone = request.Phone;
        client.CompanyName = request.CompanyName;
        client.Notes = request.Notes;

        await _context.SaveChangesAsync();

        return new ClientResponse
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            CompanyName = client.CompanyName,
            Notes = client.Notes,
            CreatedAt = client.CreatedAt
        };
    }

    public async Task DeleteClientAsync(Guid id, Guid userId)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
} 