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


// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));


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


// CORS - Allow all frontends to access the API
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


// Swagger available in Development + Production
app.UseSwagger();

app.UseSwaggerUI();


// CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.MapControllers();


// Create database schema + seed data automatically
// EnsureCreated keeps first setup simple: no migration command is required.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await db.Database.EnsureCreatedAsync();
    await DbSeeder.SeedAsync(db);
}

// Opening the API root redirects directly to Swagger.
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();