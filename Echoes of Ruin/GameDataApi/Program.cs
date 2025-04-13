using GameDataApi.Models;
using GameDataApi.Service;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("GameDatabase"));

builder.Services.AddSingleton<MongoDbService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
    Console.WriteLine("Development environment detected: Swagger UI enabled at /swagger");

    try
    {
        var dbSettings = app.Services.GetRequiredService<IOptions<DatabaseSettings>>().Value;
        if (string.IsNullOrEmpty(dbSettings.ConnectionString))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("FATAL ERROR: Database ConnectionString is NOT loaded! Check User Secrets configuration ('dotnet user-secrets set ...').");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Database ConnectionString loaded successfully from User Secrets (Value hidden for security).");
            Console.WriteLine($"Target Database: {dbSettings.DatabaseName}, Collection: {dbSettings.PlayersCollectionName}");
            Console.ResetColor();
        }
    }
    catch (Exception ex)
    {
         Console.ForegroundColor = ConsoleColor.Red;
         Console.WriteLine($"Error accessing DatabaseSettings: {ex.Message}");
         Console.ResetColor();
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();