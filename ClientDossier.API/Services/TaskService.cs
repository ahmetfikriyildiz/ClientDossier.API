using ClientDossier.API.Data;
using ClientDossier.API.DTOs;
using ClientDossier.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientDossier.API.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskResponse>> GetTasksAsync(Guid clientId, Guid userId)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        return await _context.Tasks
            .Where(t => t.ClientId == clientId)
            .Select(t => new TaskResponse
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                DueDate = t.DueDate,
                CreatedAt = t.CreatedAt,
                UserName = t.User.Name
            })
            .ToListAsync();
    }

    public async Task<TaskResponse> GetTaskByIdAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            throw new KeyNotFoundException("Task not found");

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == task.ClientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UserName = task.User.Name
        };
    }

    public async Task<TaskResponse> CreateTaskAsync(Guid clientId, CreateTaskRequest request, Guid userId)
    {
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        var task = new TaskItem
        {
            ClientId = clientId,
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = request.DueDate
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        var user = await _context.Users.FindAsync(userId);
        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UserName = user!.Name
        };
    }

    public async Task<TaskResponse> UpdateTaskAsync(Guid id, CreateTaskRequest request, Guid userId)
    {
        var task = await _context.Tasks
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            throw new KeyNotFoundException("Task not found");

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == task.ClientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.DueDate = request.DueDate;

        await _context.SaveChangesAsync();

        return new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UserName = task.User.Name
        };
    }

    public async Task DeleteTaskAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            throw new KeyNotFoundException("Task not found");

        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == task.ClientId && c.UserId == userId);

        if (client == null)
            throw new KeyNotFoundException("Client not found");

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
} 