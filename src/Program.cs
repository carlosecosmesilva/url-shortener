using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;
using UrlShortener.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=urls.db"));

builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "URL Shortener API", Version = "v1" });
});

var app = builder.Build();

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

    var shortUrl = await urlService.CreateShortUrlAsync(url);
    return Results.Ok(new { originalUrl = url, shortCode = shortUrl.ShortCode });
})
.WithName("ShortenUrl")
.WithOpenApi();

app.MapGet("/{shortCode}", async (IUrlService urlService, string shortCode) =>
{
    var url = await urlService.GetByShortCodeAsync(shortCode);
    if (url == null)
        return Results.NotFound();

    return Results.Redirect(url.OriginalUrl);
})
.WithName("RedirectUrl")
.WithOpenApi();

app.Run();
