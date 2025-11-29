using System.Reflection;
using DesafioBlue.Application.Interface.Persistence.Repository;
using DesafioBlue.Infrastructure.Persistence.Context;
using DesafioBlue.Infrastructure.Persistence.Repository;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Use Swashbuckle (Swagger) for OpenAPI UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
}

builder.Services.AddDbContext<ContactContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:8080")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Habilita Swagger UI em Development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DesafioBlue v1");
        // c.RoutePrefix = string.Empty; // descomente para servir a UI na raiz (/)
    });
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ContactContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetService<ILogger<Program>>();
        logger?.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
