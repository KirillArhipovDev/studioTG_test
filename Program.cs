using WebApi.Factories;
using WebApi.Options;
using WebApi.Repositorys;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<CellValueOptions>().Bind(builder.Configuration.GetSection("CellValueOptions"));
builder.Services.AddOptions<GameBoardOptions>().Bind(builder.Configuration.GetSection("GameBoardOptions"));
WebApiTestConfigurationOptions webApiTestConfigurationOptions = new();
builder.Configuration.GetSection(nameof(WebApiTestConfigurationOptions)).Bind(webApiTestConfigurationOptions);

builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddScoped<IGameFactory, GameFactory>();

builder.Services.AddScoped<IGameRepository, GameRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins(webApiTestConfigurationOptions.TestUrl)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowLocalhost");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
