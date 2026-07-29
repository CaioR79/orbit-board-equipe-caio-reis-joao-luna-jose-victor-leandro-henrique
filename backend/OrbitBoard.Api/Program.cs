using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using OrbitBoard.Api.Middleware;
using OrbitBoard.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OrbitBoard API",
        Version = "v1",
        Description = "API didática para gestão de projetos, tarefas e equipe."
    });
});

builder.Services.AddSingleton<IWorkspaceService, WorkspaceService>();

// Lê as origens permitidas de configuração.
// Formato aceito: uma ou várias origens separadas por vírgula.
// Se nada for definido, mantém localhost:5173 como padrão.
var corsOrigins = builder.Configuration["Cors:AllowedOrigins"]
    ?? "http://localhost:5173";

var allowedOrigins = corsOrigins
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)   // agora vem de configuração, não fixo
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("Frontend");
app.MapControllers();

app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    service = "OrbitBoard.Api",
    utcTime = DateTimeOffset.UtcNow
})).WithTags("Health");

app.Run();
