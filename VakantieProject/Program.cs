using Microsoft.EntityFrameworkCore;
using VakantieProject.Data;
using VakantieProject.RabbitMQServices;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.WebHost.UseUrls("http://*:5037");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

/*var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
builder.Services.AddScoped<IHotelRepo, HotelRepo>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();*/
builder.Services.AddScoped<IHotelRepo, HotelRepo>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

var connectionString = builder.Configuration.GetConnectionString("SqlConnectionString");
Debug.WriteLine(connectionString + " the shit con");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));


ConfigureLogging();
Log.Information("Logging has been configured successfully.");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    Debug.WriteLine("START THE FACKING SWAGGER");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureLogging()
{
    var elasticUri = builder.Configuration["ElasticConfiguration:Uri"];

    Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithExceptionDetails()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
        {
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            IndexFormat = $"banana",
            NumberOfShards = 2,
            NumberOfReplicas = 1
        })
        .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
        .Enrich.WithProperty("Application", "VakantieProject")
        .CreateLogger();

    builder.Host.UseSerilog();
}
