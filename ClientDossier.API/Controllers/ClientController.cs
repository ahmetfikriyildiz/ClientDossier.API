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
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClientController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientResponse>>> GetClients()
    {
        var userId = GetCurrentUserId();
        var clients = await _context.Clients
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

        return Ok(clients);
    }

    [HttpPost]
    public async Task<ActionResult<ClientResponse>> CreateClient(CreateClientRequest request)
    {
        var userId = GetCurrentUserId();
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

        return Ok(new ClientResponse
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            CompanyName = client.CompanyName,
            Notes = client.Notes,
            CreatedAt = client.CreatedAt
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClientResponse>> UpdateClient(Guid id, UpdateClientRequest request)
    {
        var userId = GetCurrentUserId();
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (client == null)
        {
            return NotFound();
        }

        client.Name = request.Name;
        client.Email = request.Email;
        client.Phone = request.Phone;
        client.CompanyName = request.CompanyName;
        client.Notes = request.Notes;

        await _context.SaveChangesAsync();

        return Ok(new ClientResponse
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            Phone = client.Phone,
            CompanyName = client.CompanyName,
            Notes = client.Notes,
            CreatedAt = client.CreatedAt
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        var userId = GetCurrentUserId();
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (client == null)
        {
            return NotFound();
        }

        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();

        return NoContent();
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