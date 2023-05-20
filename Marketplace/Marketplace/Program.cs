using Marketplace.Configurations;
using Marketplace.Controllers;
using Marketplace.Executors;
using Marketplace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DatabaseConfiguration>();
builder.Services.AddIdentity<Users, IdentityRole>()
    .AddEntityFrameworkStores<DatabaseConfiguration>()
    .AddDefaultTokenProviders();
builder.Services.AddTransient<InitialDataSeeder>();

builder.Services.AddScoped<UsersQueryExecutor>();
builder.Services.AddScoped<AuthQueryExecutor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DatabaseConfiguration>();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while querying the database.");
    }
}


using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var seeder = serviceScope.ServiceProvider.GetService<InitialDataSeeder>();
    seeder.SeedData().Wait();
}

app.Run();
