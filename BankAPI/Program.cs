using BankAPI.Models; 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddSingleton<Bank>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}
else
{
    // Enable static file hosting for Vite in production
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Optional: fallback to index.html for SPA routing
if (!app.Environment.IsDevelopment())
{
    app.MapFallbackToFile("index.html");
}

app.Run();