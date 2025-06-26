# RoverMissionPlaner

Rover Mission Planner

Microservicio para planificación y validación de tareas de exploradores rover en Marte.  
Diseñado para uso inmediato por operarios de Mission Control Chile.

---

Tecnologías utilizadas

- .NET 8
- Arquitectura en capas (Domain, Application, Infrastructure, API)
- FluentValidation para validación de comandos
- In-Memory Repository
- XUnit para pruebas
- Swagger para exploración de endpoints

---

Instalación y ejecución

Desde la raíz del proyecto, ejecuta:

```bash
dotnet build
dotnet test
dotnet run --project RoverMissionPlanner.API

La API estará disponible en:

https://localhost:5001/swagger
