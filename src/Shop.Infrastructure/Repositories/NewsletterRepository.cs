using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;
using Shop.Infrastructure.Persistence;

namespace Shop.Infrastructure.Repositories;

public class NewsletterRepository : INewsletterRepository
{
    private readonly ApplicationDbContext _db;

    public NewsletterRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public Task<bool> ExistsAsync(string email) =>
        _db.NewsletterSubscriptions.AnyAsync(x => x.Email == email);

    public async Task AddAsync(NewsletterSubscription subscription)
    {
        _db.NewsletterSubscriptions.Add(subscription);
        await _db.SaveChangesAsync();
    }
}
