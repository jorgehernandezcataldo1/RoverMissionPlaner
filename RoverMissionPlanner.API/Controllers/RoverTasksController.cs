using Microsoft.AspNetCore.Mvc;
using RoverMissionPlanner.Application.Commands;
using RoverMissionPlanner.Infrastructure.Repositories;
using RoverMissionPlanner.Domain.Entities;

namespace RoverMissionPlanner.API.Controllers;

[ApiController]
[Route("rovers/{roverName}/tasks")]
public class RoverTasksController : ControllerBase
{
    private readonly IRoverTaskRepository _repo;
    private readonly CreateRoverTaskCommandHandler _createHandler;

    public RoverTasksController(
        IRoverTaskRepository repo,
        CreateRoverTaskCommandHandler createHandler)
    {
        _repo = repo;
        _createHandler = createHandler;
    }

    //---- POST ----// 
    [HttpPost]
    public async Task<IActionResult> Create(string roverName, [FromBody] CreateRoverTaskCommand command)
    {
        try
        {
            command.RoverName = roverName;

            var task = await _createHandler.HandleAsync(command);

            return CreatedAtAction(nameof(GetTasks), new { roverName, date = command.StartsAt.Date }, task);
        }
        catch (InvalidOperationException)
        {
            return Conflict("Task overlaps with an existing one.");
        }
    }

    //---- GET ---//
    [HttpGet]
    public async Task<IActionResult> GetTasks(string roverName, [FromQuery] DateOnly date)
    {
        var tasks = await _repo.GetTasksAsync(roverName, date);
        return Ok(tasks);
    }

    //---- GET ------//
    [HttpGet("/rovers/{roverName}/utilization")]
    public async Task<IActionResult> GetUtilization(string roverName, [FromQuery] DateOnly date)
    {
        var utilization = await _repo.GetUtilizationAsync(roverName, date);
        return Ok(new { utilization = Math.Round(utilization * 100, 2) }); // %
    }
}
