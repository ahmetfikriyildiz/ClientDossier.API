using ClientDossier.API.DTOs;
using ClientDossier.API.Models;

namespace ClientDossier.API.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponse>> GetTasksAsync(Guid clientId, Guid userId);
    Task<TaskResponse> GetTaskByIdAsync(Guid id, Guid userId);
    Task<TaskResponse> CreateTaskAsync(Guid clientId, CreateTaskRequest request, Guid userId);
    Task<TaskResponse> UpdateTaskAsync(Guid id, CreateTaskRequest request, Guid userId);
    Task DeleteTaskAsync(Guid id, Guid userId);
} 