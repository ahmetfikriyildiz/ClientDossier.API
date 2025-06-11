using ClientDossier.API.Data;
using ClientDossier.API.DTOs;
using ClientDossier.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientDossier.API.Services;

public class DocumentService : IDocumentService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public DocumentService(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<IEnumerable<DocumentResponse>> GetDocumentsAsync(Guid clientId, Guid userId)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        return await _context.Documents
            .Where(d => d.ClientId == clientId)
            .Select(d => new DocumentResponse
            {
                Id = d.Id,
                FileName = d.FileName,
                FileUrl = d.FileUrl,
                UploadedAt = d.UploadedAt
            })
            .ToListAsync();
    }

    public async Task<DocumentResponse> GetDocumentByIdAsync(Guid id, Guid userId)
    {
        var document = await _context.Documents
            .FirstOrDefaultAsync(d => d.Id == id);

        if (document == null)
            throw new KeyNotFoundException("Document not found");

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == document.ClientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        return new DocumentResponse
        {
            Id = document.Id,
            FileName = document.FileName,
            FileUrl = document.FileUrl,
            UploadedAt = document.UploadedAt
        };
    }

    public async Task<DocumentResponse> UploadDocumentAsync(Guid clientId, CreateDocumentRequest request, Guid userId)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        if (request.File == null || request.File.Length == 0)
            throw new ArgumentException("No file uploaded");

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

        return new DocumentResponse
        {
            Id = document.Id,
            FileName = document.FileName,
            FileUrl = document.FileUrl,
            UploadedAt = document.UploadedAt
        };
    }

    public async Task DeleteDocumentAsync(Guid id, Guid userId)
    {
        var document = await _context.Documents
            .FirstOrDefaultAsync(d => d.Id == id);

        if (document == null)
            throw new KeyNotFoundException("Document not found");

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == document.ClientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        var filePath = Path.Combine(_environment.WebRootPath, document.FileUrl.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        _context.Documents.Remove(document);
        await _context.SaveChangesAsync();
    }
} 