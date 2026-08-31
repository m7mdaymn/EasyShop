namespace Shop.Application.Interfaces;

public interface INewsletterService
{
    Task<bool> SubscribeAsync(string email);
}
