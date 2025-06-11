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
[Route("api/clients/{clientId}/tasks")]
public class TaskController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasks(Guid clientId)
    {
        var userId = GetCurrentUserId();
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
        {
            return NotFound("Client not found");
        }

        var tasks = await _context.Tasks
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

        return Ok(tasks);
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponse>> CreateTask(Guid clientId, CreateTaskRequest request)
    {
        var userId = GetCurrentUserId();
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == clientId && c.UserId == userId);

        if (client == null)
        {
            return NotFound("Client not found");
        }

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
        return Ok(new TaskResponse
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UserName = user!.Name
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