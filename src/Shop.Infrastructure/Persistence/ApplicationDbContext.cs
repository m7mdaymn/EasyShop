using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductTag> ProductTags => Set<ProductTag>();
    public DbSet<Review> Reviews => Set<Review>();

    public DbSet<NewsletterSubscription> NewsletterSubscriptions
        => Set<NewsletterSubscription>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Slug)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Image)
                .HasMaxLength(500);

            entity.HasIndex(x => x.Name)
                .IsUnique();

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.HasMany(x => x.Products)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(2000)
                .IsRequired();

            entity.Property(x => x.Price)
                .HasColumnType("decimal(10,2)");

            entity.Property(x => x.Brand)
                .HasMaxLength(150);

            entity.Property(x => x.Thumbnail)
                .HasMaxLength(500);

            entity.HasMany(x => x.Images)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Tags)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.Reviews)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        // ProductImage
        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ImageUrl)
                .HasMaxLength(500)
                .IsRequired();
        });


        // ProductTag
        modelBuilder.Entity<ProductTag>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => new
            {
                x.ProductId,
                x.Name
            })
            .IsUnique();
        });


        // Review
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ReviewerName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Comment)
                .HasMaxLength(1000);

            entity.ToTable(table =>
                table.HasCheckConstraint(
                    "CK_Reviews_Rating",
                    "[Rating] >= 1 AND [Rating] <= 5"));
        });


        // Newsletter
        modelBuilder.Entity<NewsletterSubscription>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(x => x.Email)
                .IsUnique();
        });
    }
}