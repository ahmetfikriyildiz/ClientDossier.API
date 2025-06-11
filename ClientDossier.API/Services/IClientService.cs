using ClientDossier.API.DTOs;
using ClientDossier.API.Models;

namespace ClientDossier.API.Services;

public interface IClientService
{
    Task<IEnumerable<ClientResponse>> GetClientsAsync(Guid userId);
    Task<ClientResponse> GetClientByIdAsync(Guid id, Guid userId);
    Task<ClientResponse> CreateClientAsync(CreateClientRequest request, Guid userId);
    Task<ClientResponse> UpdateClientAsync(Guid id, UpdateClientRequest request, Guid userId);
    Task DeleteClientAsync(Guid id, Guid userId);
} 