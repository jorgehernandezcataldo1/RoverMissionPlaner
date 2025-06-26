using FluentValidation;

namespace RoverMissionPlanner.Application.Commands;

public class CreateRoverTaskCommandValidator : AbstractValidator<CreateRoverTaskCommand>
{
    public CreateRoverTaskCommandValidator()
    {
        RuleFor(x => x.RoverName).NotEmpty();
        RuleFor(x => x.TaskType).IsInEnum();
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        RuleFor(x => x.StartsAt).NotEmpty();
        RuleFor(x => x.DurationMinutes).GreaterThan(0);
    }
}
