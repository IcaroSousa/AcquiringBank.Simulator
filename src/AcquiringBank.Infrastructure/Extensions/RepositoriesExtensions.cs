using AcquiringBank.Data.Repositories;
using AcquiringBank.Data.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AcquiringBank.Infrastructure.Extensions;

public static class RepositoriesExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAccountRepository, AccountRepository>();
        serviceCollection.AddScoped<ICardRepository, CardRepository>();
        serviceCollection.AddScoped<ITransactionRepository, TransactionRepository>();

        return serviceCollection;
    }
}