using System.Reflection;
using System.Security.Cryptography;
using API.Helpers;
using Core.Settings;
using Database;
using EventConsumer;
using EventPublisher;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Service.File;
using Service.FileStorage;
using Service.Generator;
using Service.PdfFile;

var builder = WebApplication.CreateBuilder(args);
var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();

builder.Services.Configure<ExtensionSettings>(builder.Configuration.GetSection("ExtensionSettings"));
builder.Services.Configure<FilesStorageSettings>(builder.Configuration.GetSection("FilesDirectorySettings"));

// predefine directories
{
    var filesDirectory = builder.Configuration.GetSection("FilesDirectorySettings").Get<FilesStorageSettings>();

    if (!Directory.Exists(filesDirectory!.InputFileBaseDirectory))
        Directory.CreateDirectory(filesDirectory.InputFileBaseDirectory);

    if (!Directory.Exists(filesDirectory!.OutputFileBaseDirectory))
        Directory.CreateDirectory(filesDirectory.OutputFileBaseDirectory);
}

builder.Services.AddCors();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.InputFormatterMemoryBufferThreshold = 1024 * 1024 * 30;
    options.OutputFormatterMemoryBufferThreshold = 1024 * 1024 * 30;
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new ApiDateTimeConverter(
        "yyyy-MM-dd HH:mm:ss",
        new []
        {
            "yyyy-MM-dd",
            "yyyy-MM-dd HH:mm:ss"
        }
    ));
});

// Database - SQLite
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(cs);
});

// EventBus
builder.Services.Configure<EventBusSettings>(builder.Configuration.GetSection("EventBusSettings"));

// You can switch on message brokers anytime when it is needed
builder.Services.AddHostedService<RabbitMqConsumer>();
builder.Services.AddScoped<RabbitMqPublisher>();

// Services
builder.Services.AddScoped<InputFileService>();
builder.Services.AddScoped<PdfGeneratorService>();
builder.Services.AddScoped<PdfFileService>();
builder.Services.AddScoped<FileStorageService>();

// Swagger(OpenAPI)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "File Board API",
        Description = "An ASP.NET Core Web API for managing files",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Made by Azam",
            Url = new Uri("https://www.linkedin.com/in/azam-turgunov/")
        },
        License = new OpenApiLicense
        {
            Name = "Open License",
            Url = new Uri("https://example.com/license")
        }
    });
    
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// builder.Services.AddHostedService<Seed>();

var app = builder.Build();
app.UseRouting();
app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyMethod();
    policyBuilder.AllowCredentials();
    policyBuilder.AllowAnyHeader();
    policyBuilder.WithOrigins(corsSettings.AllowedOrigins);
    policyBuilder.SetPreflightMaxAge(TimeSpan.FromDays(1));
});
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();