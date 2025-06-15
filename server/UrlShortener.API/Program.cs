using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UrlShortener.API.Data;
using UrlShortener.API.Repositories;
using UrlShortener.API.Services;
using MassTransit;
using UrlShortener.API.Messaging.Consumers;

var builder = WebApplication.CreateBuilder(args);

// MySQL connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "URL Shortener API",
        Version = "v1",
        Description = "API for creating and managing short URLs.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Your Name",
            Email = "your@email.com"
        }
    });
});
builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddScoped<ShortUrlService>(sp =>
    new ShortUrlService(
        sp.GetRequiredService<IShortUrlRepository>(),
        sp.GetRequiredService<MassTransit.IPublishEndpoint>(),
        sp.GetRequiredService<IConfiguration>()));
builder.Services.AddScoped<JwtService>();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ShortUrlCreatedConsumer>();
    x.AddConsumer<ShortUrlDeletedConsumer>();
    x.AddConsumer<ShortUrlCreationFailedConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("short-url-created-queue", e =>
        {
            e.ConfigureConsumer<ShortUrlCreatedConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("short-url-deleted-queue", e =>
        {
            e.ConfigureConsumer<ShortUrlDeletedConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("short-url-failed-queue", e =>
        {
            e.ConfigureConsumer<ShortUrlCreationFailedConsumer>(ctx);
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
