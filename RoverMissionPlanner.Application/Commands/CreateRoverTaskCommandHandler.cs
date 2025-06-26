using FluentValidation;
using RoverMissionPlanner.Domain.Entities;
using RoverMissionPlanner.Infrastructure.Repositories;

namespace RoverMissionPlanner.Application.Commands;

public class CreateRoverTaskCommandHandler
{
    private readonly IRoverTaskRepository _repo;
    private readonly CreateRoverTaskCommandValidator _validator;

    public CreateRoverTaskCommandHandler(
        IRoverTaskRepository repo,
        CreateRoverTaskCommandValidator validator)
    {
        _repo = repo;
        _validator = validator;
    }

    public async Task<RoverTask> HandleAsync(CreateRoverTaskCommand command)
    {
        var result = _validator.Validate(command);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var end = command.StartsAt.AddMinutes(command.DurationMinutes);

        var overlap = await _repo.HasOverlapAsync(command.RoverName, command.StartsAt, end);
        if (overlap)
        {
            throw new InvalidOperationException("Task overlaps with an existing one.");
        }

        var task = new RoverTask
        {
            RoverName = command.RoverName,
            TaskType = command.TaskType,
            Latitude = command.Latitude,
            Longitude = command.Longitude,
            StartsAt = command.StartsAt,
            DurationMinutes = command.DurationMinutes,
            Status = RoverMissionPlanner.Domain.Entities.TaskStatus.Planned
        };

        var addedTask = await _repo.AddTaskAsync(task);
        if (addedTask is null)
        {
            throw new InvalidOperationException("Failed to add the task.");
        }

        return addedTask;
    }
}

