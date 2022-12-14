using Minesweeper.Server.Entities;
using Minesweeper.Server.HubConfig;
using Minesweeper.Server.repository;
using Minesweeper.Server.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddSingleton<IPlayFieldRepository, PlayFieldRepository>();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var mongo = new MongoClient("mongodb://localhost:27017");

builder.Services.AddSingleton<IMongoClient>(mongo);
builder.Services.AddSingleton<IMongoDatabase>(services => services.GetRequiredService<IMongoClient>().GetDatabase("minesweeper"));
builder.Services.AddSingleton<IMongoCollection<PlayField>>(services => services.GetRequiredService<IMongoDatabase>().GetCollection<PlayField>("playfields"));

var app = builder.Build();

app.MapControllers();
app.MapHub<GameHub>("/game");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();