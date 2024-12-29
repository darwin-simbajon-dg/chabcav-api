var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

Console.WriteLine($"Application is running on port: {Environment.GetEnvironmentVariable("PORT")}");


app.Urls.Add($"http://*:{Environment.GetEnvironmentVariable("PORT")}");

app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.MapGet("/hello", () => "Hello, World!");

app.Run();
