using chabcav.application.Commands.AddConfiguration;
using chabcav.application.Commands.GetConfiguration;
using chabcav.application.Commands.RegisterUser;
using chabcav.domain.Interfaces;
using chabcav.domain.Services;
using chabcav.infrastructure.Data.Repositories;
using chabcav_api.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
//});

var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");

builder.Services.AddMediatR(typeof(RegisterUserCommand).Assembly);
builder.Services.AddMediatR(typeof(AddConfigurationCommand).Assembly);
builder.Services.AddMediatR(typeof(GetConfigurationCommand).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Add your frontend origin here
              .AllowAnyHeader()                  // Allow all headers
              .AllowAnyMethod();                 // Allow all HTTP methods (GET, POST, etc.)
    });

    options.AddPolicy("AllRailway", policy =>
    {
        policy.WithOrigins("https://chabcav-api-development.up.railway.app") // Add your frontend origin here
              .AllowAnyHeader()                  // Allow all headers
              .AllowAnyMethod();                 // Allow all HTTP methods (GET, POST, etc.)
    });
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IDbConnection>(connection =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("PostgresConnection")));
}
else 
{
    builder.Services.AddSingleton<IDbConnection>(connection =>
    new NpgsqlConnection(connectionString));
}



builder.Services.AddScoped<IUserStore<IdentityUser>, UserStore>();
builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleStore>();
builder.Services.AddScoped<ICMSRepository, CMSRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();



builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders();

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

Console.WriteLine($"Application is running on port: {Environment.GetEnvironmentVariable("PORT")}");

app.UseCors("AllowFrontend");
app.UseCors("AllRailway");


app.Urls.Add($"http://*:{Environment.GetEnvironmentVariable("PORT")}");

app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.MapGet("/hello", () => "Hello, World!");

app.MapUserEndpoints();
app.MapCMSEndpoints();

app.Run();
