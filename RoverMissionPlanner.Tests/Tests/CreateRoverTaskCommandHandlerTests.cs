
using RoverMissionPlanner.Application.Commands;
using RoverMissionPlanner.Infrastructure.Repositories;
using RoverMissionPlanner.Domain.Entities;



namespace RoverMissionPlanner.Tests;

public class CreateRoverTaskCommandHandlerTests
{
    [Fact]
public async Task Should_Create_Task_When_No_Overlap()
{
    
    var repo = new InMemoryRoverTaskRepository();
    var validator = new CreateRoverTaskCommandValidator();
    var handler = new CreateRoverTaskCommandHandler(repo, validator);

    var command = new CreateRoverTaskCommand
    {
        RoverName = "Spirit",
        TaskType = TaskType.Photo,
        Latitude = 1.23,
        Longitude = 4.56,
        StartsAt = new DateTime(2025, 6, 25, 10, 0, 0),
        DurationMinutes = 60
    };

    
    var result = await handler.HandleAsync(command);

    
    Assert.NotNull(result);
    Assert.Equal(command.RoverName, result.RoverName);
}


    [Fact]
public async Task Should_Throw_When_Task_Overlaps()
{
    // Arrange
    var repo = new InMemoryRoverTaskRepository();
    var validator = new CreateRoverTaskCommandValidator();
    var handler = new CreateRoverTaskCommandHandler(repo, validator);

    var firstTask = new CreateRoverTaskCommand
    {
        RoverName = "Opportunity",
        TaskType = TaskType.Drill,
        Latitude = 0,
        Longitude = 0,
        StartsAt = new DateTime(2025, 6, 25, 10, 0, 0),
        DurationMinutes = 60
    };

    var overlappingTask = new CreateRoverTaskCommand
    {
        RoverName = "Opportunity",
        TaskType = TaskType.Sample,
        Latitude = 0,
        Longitude = 0,
        StartsAt = new DateTime(2025, 6, 25, 10, 30, 0), // dentro del rango
        DurationMinutes = 30
    };

    await handler.HandleAsync(firstTask);

    
    await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(overlappingTask));
}

}
