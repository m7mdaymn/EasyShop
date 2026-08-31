using System.Net.Mail;
using Shop.Application.Interfaces;
using Shop.Application.Interfaces.Repositories;
using Shop.Domain.Entities;

namespace Shop.Application.Services;

public class NewsletterService : INewsletterService
{
    private readonly INewsletterRepository _newsletter;

    public NewsletterService(INewsletterRepository newsletter)
    {
        _newsletter = newsletter;
    }

    public async Task<bool> SubscribeAsync(string email)
    {
        email = email.Trim().ToLowerInvariant();
        _ = new MailAddress(email);

        if (await _newsletter.ExistsAsync(email))
            return false;

        await _newsletter.AddAsync(new NewsletterSubscription
        {
            Email = email,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        });

        return true;
    }
}
