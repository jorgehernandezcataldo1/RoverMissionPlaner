using RoverMissionPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoverMissionPlanner.Infrastructure.Repositories;

public interface IRoverTaskRepository
{
    Task<IEnumerable<RoverTask>> GetTasksAsync(string roverName, DateOnly date);
    Task<RoverTask?> AddTaskAsync(RoverTask task);
    Task<bool> HasOverlapAsync(string roverName, DateTime start, DateTime end);
    Task<double> GetUtilizationAsync(string roverName, DateOnly date);
}

public class InMemoryRoverTaskRepository : IRoverTaskRepository
{
    private readonly List<RoverTask> _tasks = new();

    public Task<RoverTask?> AddTaskAsync(RoverTask task)
    {
        _tasks.Add(task);
        return Task.FromResult<RoverTask?>(task);
    }

    public Task<IEnumerable<RoverTask>> GetTasksAsync(string roverName, DateOnly date)
    {
        var tasks = _tasks
            .Where(t => t.RoverName.Equals(roverName, StringComparison.OrdinalIgnoreCase)
                && DateOnly.FromDateTime(t.StartsAt) == date)
            .OrderBy(t => t.StartsAt)
            .AsEnumerable();

        return Task.FromResult(tasks);
    }

    public Task<bool> HasOverlapAsync(string roverName, DateTime start, DateTime end)
    {
        var overlapping = _tasks.Any(t =>
            t.RoverName.Equals(roverName, StringComparison.OrdinalIgnoreCase) &&
            t.StartsAt < end &&
            t.EndsAt > start
        );

        return Task.FromResult(overlapping);
    }

    public async Task<double> GetUtilizationAsync(string roverName, DateOnly date)
    {
        var tasks = await GetTasksAsync(roverName, date);
        var totalMinutes = tasks.Sum(t => t.DurationMinutes);
        return totalMinutes / 1440.0; // 1440 minutos = 1 día
    }
}