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
[Route("api/clients/{clientId}/documents")]
public class DocumentController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public DocumentController(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DocumentResponse>>> GetDocuments(Guid clientId)
    {
        var userId = GetCurrentUserId();
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
        {
            return NotFound("Client not found");
        }

        var documents = await _context.Documents
            .Where(d => d.ClientId == clientId)
            .Select(d => new DocumentResponse
            {
                Id = d.Id,
                FileName = d.FileName,
                FileUrl = d.FileUrl,
                UploadedAt = d.UploadedAt
            })
            .ToListAsync();

        return Ok(documents);
    }

    [HttpPost]
    public async Task<ActionResult<DocumentResponse>> UploadDocument(Guid clientId, [FromForm] CreateDocumentRequest request)
    {
        var userId = GetCurrentUserId();
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
        {
            return NotFound("Client not found");
        }

        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", clientId.ToString());
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName = $"{Guid.NewGuid()}_{request.FileName}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        var document = new Document
        {
            ClientId = clientId,
            FileName = request.FileName,
            FileUrl = $"/uploads/{clientId}/{fileName}"
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return Ok(new DocumentResponse
        {
            Id = document.Id,
            FileName = document.FileName,
            FileUrl = document.FileUrl,
            UploadedAt = document.UploadedAt
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