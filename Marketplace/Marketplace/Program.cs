using Marketplace.Configurations;
using Marketplace.Controllers;
using Marketplace.Executors;
using Marketplace.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder().AddJsonFile(Environment.CurrentDirectory + "/JWTBearer.json").Build();

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
builder.Services.AddScoped<UserActionsQueryExecutor>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(UserActionLoggingActionFilter));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JWTBearer:Issuer"],
        ValidAudience = config["JWTBearer:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTBearer:Key"]))
    };
});

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
