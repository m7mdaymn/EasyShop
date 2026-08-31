using Microsoft.EntityFrameworkCore;
using Shop.Domain.Entities;

namespace Shop.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(
        ApplicationDbContext db)
    {
        await SeedCategories(db);

        await SeedProducts(db);

        await SeedNewsletter(db);
    }


    private static async Task SeedCategories(
        ApplicationDbContext db)
    {
        if (await db.Categories.AnyAsync())
            return;


        var categoryNames = new[]
        {
            "Beauty",
            "Fragrances",
            "Furniture",
            "Groceries",
            "Home Decoration",
            "Kitchen Accessories",
            "Laptops",
            "Men's Shirts",
            "Men's Shoes",
            "Men's Watches",
            "Mobile Accessories",
            "Motorcycle",
            "Skin Care",
            "Smartphones",
            "Sports Accessories",
            "Sunglasses",
            "Tablets",
            "Tops",
            "Vehicle",
            "Women's Bags",
            "Women's Dresses",
            "Women's Jewellery",
            "Women's Shoes",
            "Women's Watches"
        };


        var categories = categoryNames
            .Select((name, index) => new Category
            {
                Name = name,

                Slug = ToSlug(name),

                Image =
                    $"https://picsum.photos/seed/category-{index + 1}/800/500",

                IsActive = true
            })
            .ToList();


        db.Categories.AddRange(categories);

        await db.SaveChangesAsync();
    }


    private static async Task SeedProducts(
        ApplicationDbContext db)
    {
        if (await db.Products.AnyAsync())
            return;


        var categories = await db.Categories
            .OrderBy(x => x.Id)
            .ToListAsync();


        var reviewNames = new[]
        {
            "Alex Morgan",
            "Sara Ahmed",
            "Omar Hassan"
        };


        var comments = new[]
        {
            "Great product and good value for money.",

            "Very good quality and matched the description.",

            "Good product. I would buy it again."
        };


        var products = new List<Product>();

        var productNumber = 1;


        foreach (var category in categories)
        {
            for (var i = 1; i <= 5; i++)
            {
                var product = new Product
                {
                    Title =
                        $"{category.Name} Product {i}",

                    Description =
                        $"A high quality {category.Name.ToLowerInvariant()} product prepared for the Shop catalog.",

                    Price = Math.Round(
                        8m +
                        category.Id * 1.75m +
                        i * 3.40m,
                        2),

                    Stock =
                        8 +
                        ((category.Id * 7 + i * 11) % 70),

                    Brand =
                        $"{category.Name} Co.",

                    Thumbnail =
                        $"https://picsum.photos/seed/shop-product-{productNumber}/700/700",

                    IsFeatured = i == 1,

                    IsActive = true,

                    CreatedAt =
                        DateTime.UtcNow.AddDays(
                            -productNumber),

                    CategoryId =
                        category.Id
                };


                // Images

                product.Images.Add(
                    new ProductImage
                    {
                        ImageUrl =
                            $"https://picsum.photos/seed/shop-product-{productNumber}-1/900/900",

                        Order = 1
                    });


                product.Images.Add(
                    new ProductImage
                    {
                        ImageUrl =
                            $"https://picsum.photos/seed/shop-product-{productNumber}-2/900/900",

                        Order = 2
                    });


                product.Images.Add(
                    new ProductImage
                    {
                        ImageUrl =
                            $"https://picsum.photos/seed/shop-product-{productNumber}-3/900/900",

                        Order = 3
                    });


                // Tags

                product.Tags.Add(
                    new ProductTag
                    {
                        Name = category.Slug
                    });


                product.Tags.Add(
                    new ProductTag
                    {
                        Name = i % 2 == 0
                            ? "popular"
                            : "new"
                    });


                // Reviews

                for (var r = 0; r < 3; r++)
                {
                    product.Reviews.Add(
                        new Review
                        {
                            ReviewerName =
                                reviewNames[r],

                            Rating =
                                3 +
                                ((productNumber + r) % 3),

                            Comment =
                                comments[r],

                            CreatedAt =
                                DateTime.UtcNow
                                    .AddDays(-(r + 1))
                        });
                }


                products.Add(product);

                productNumber++;
            }
        }


        db.Products.AddRange(products);

        await db.SaveChangesAsync();
    }


    private static async Task SeedNewsletter(
        ApplicationDbContext db)
    {
        if (await db
            .NewsletterSubscriptions
            .AnyAsync())
        {
            return;
        }


        var subscribers =
            Enumerable.Range(1, 10)
                .Select(i =>
                    new NewsletterSubscription
                    {
                        Email =
                            $"subscriber{i}@example.com",

                        CreatedAt =
                            DateTime.UtcNow.AddDays(-i),

                        IsActive = true
                    });


        db.NewsletterSubscriptions
            .AddRange(subscribers);


        await db.SaveChangesAsync();
    }


    private static string ToSlug(
        string value)
    {
        return value
            .Trim()
            .ToLowerInvariant()
            .Replace("'", string.Empty)
            .Replace(" ", "-");
    }
}