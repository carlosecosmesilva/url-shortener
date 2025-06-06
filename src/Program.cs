using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;
using UrlShortener.Services.Interfaces;
using FluentValidation;
using UrlShortener.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=urls.db"));

builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateShortUrlRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "URL Shortener API",
        Version = "v1",
        Description = "A simple URL shortener service that creates short, memorable URLs from long ones."
    });
});

var app = builder.Build();

// Ensure database is created and migrated
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// URL Shortening endpoints
app.MapPost("/shorten", async (IUrlService urlService, string url) =>
{
    if (string.IsNullOrEmpty(url))
        return Results.BadRequest("URL cannot be empty");

    try
    {
        var shortUrl = await urlService.CreateShortUrlAsync(url);
        var baseUrl = "https://localhost:5001"; // In production, this should come from configuration
        return Results.Ok(new
        {
            originalUrl = shortUrl.OriginalUrl,
            shortUrl = $"{baseUrl}/{shortUrl.ShortCode}",
            shortCode = shortUrl.ShortCode,
            createdAt = shortUrl.CreatedAt
        });
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Error creating short URL: {ex.Message}");
    }
})
.WithName("ShortenUrl")
.WithOpenApi(operation =>
{
    operation.Description = "Creates a shortened URL from a long URL";
    return operation;
});

// Catch-all route for URL redirection
app.MapGet("/{shortCode}", async (IUrlService urlService, string shortCode) =>
{
    if (string.IsNullOrEmpty(shortCode))
        return Results.BadRequest("Short code cannot be empty");

    try
    {
        var url = await urlService.GetByShortCodeAsync(shortCode);
        if (url == null)
            return Results.NotFound("Short URL not found");

        return Results.Redirect(url.OriginalUrl);
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"Error redirecting: {ex.Message}");
    }
})
.WithName("RedirectUrl")
.WithOpenApi(operation =>
{
    operation.Description = "Redirects to the original URL associated with the short code";
    return operation;
});

// Statistics endpoint
app.MapGet("/stats/{shortCode}", async (IUrlService urlService, string shortCode) =>
{
    var url = await urlService.GetByShortCodeAsync(shortCode);
    if (url == null)
        return Results.NotFound("Short URL not found");

    return Results.Ok(new
    {
        originalUrl = url.OriginalUrl,
        shortCode = url.ShortCode,
        createdAt = url.CreatedAt,
        lastAccessedAt = url.LastAccessedAt,
        clickCount = url.ClickCount
    });
})
.WithName("GetUrlStats")
.WithOpenApi(operation =>
{
    operation.Description = "Get statistics for a shortened URL";
    return operation;
});

app.Run();
