using Shop.Domain.Entities;

namespace Shop.Application.Interfaces.Repositories;

public interface INewsletterRepository
{
    Task<bool> ExistsAsync(string email);
    Task AddAsync(NewsletterSubscription subscription);
}
