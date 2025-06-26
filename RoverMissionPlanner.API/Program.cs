using RoverMissionPlanner.Infrastructure.Repositories;
using RoverMissionPlanner.Application.Commands;


var builder = WebApplication.CreateBuilder(args);

// Servicios de infraestructura y aplicación
builder.Services.AddSingleton<IRoverTaskRepository, InMemoryRoverTaskRepository>();
builder.Services.AddScoped<CreateRoverTaskCommandHandler>();
builder.Services.AddScoped<CreateRoverTaskCommandValidator>();

// Servicios API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
app.UseMiddleware<ExceptionMiddleware>();
