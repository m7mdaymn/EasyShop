using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces;
using Shop.Application.Interfaces.Repositories;
using Shop.Application.Services;
using Shop.Infrastructure.Persistence;
using Shop.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});


// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<INewsletterRepository, NewsletterRepository>();


// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<INewsletterService, NewsletterService>();


// CORS - allow any frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();


// Swagger in Development + Production
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "Shop API V1");

    options.RoutePrefix = "swagger";
});


// CORS MUST be before controllers
app.UseCors("AllowAll");


// DON'T redirect HTTP while we're testing
// app.UseHttpsRedirection();


app.MapControllers();


app.MapGet("/", () =>
{
    return Results.Redirect("/swagger");
});


app.MapGet("/health", () =>
{
    return Results.Ok(new
    {
        status = "running",
        service = "Shop API"
    });
});


app.Run();