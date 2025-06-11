using ClientDossier.API.DTOs;

namespace ClientDossier.API.Services;

public interface IInvoiceService
{
    Task<IEnumerable<InvoiceResponse>> GetInvoicesAsync(Guid clientId, Guid userId);
    Task<InvoiceResponse> GetInvoiceByIdAsync(Guid id, Guid userId);
    Task<InvoiceResponse> CreateInvoiceAsync(Guid clientId, CreateInvoiceRequest request, Guid userId);
    Task<InvoiceResponse> UpdateInvoiceAsync(Guid id, CreateInvoiceRequest request, Guid userId);
    Task DeleteInvoiceAsync(Guid id, Guid userId);
} 