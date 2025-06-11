using ClientDossier.API.DTOs;
using Microsoft.AspNetCore.Http;

namespace ClientDossier.API.Services;

public interface IDocumentService
{
    Task<IEnumerable<DocumentResponse>> GetDocumentsAsync(Guid clientId, Guid userId);
    Task<DocumentResponse> GetDocumentByIdAsync(Guid id, Guid userId);
    Task<DocumentResponse> UploadDocumentAsync(Guid clientId, CreateDocumentRequest request, Guid userId);
    Task DeleteDocumentAsync(Guid id, Guid userId);
} 