using System;
using System.Collections.Generic;
using System.Linq;
using AcquiringBank.Application.Services;
using AcquiringBank.Application.Services.Abstractions;
using AcquiringBank.Application.Services.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace AcquiringBank.Infrastructure.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransactionValidationRules();

        serviceCollection.AddScoped<IAccountService, AccountService>();
        serviceCollection.AddScoped<ICardService, CardService>();
        serviceCollection.AddScoped<IPaymentService, PaymentService>();

        return serviceCollection;
    }

    private static IServiceCollection AddTransactionValidationRules(this IServiceCollection serviceCollection)
    {
        foreach (var transactionRule in FindTransactionValidatorRules())
            serviceCollection.AddScoped(_ => transactionRule);

        return serviceCollection;
    }

    private static IEnumerable<IPaymentTransactionValidator> FindTransactionValidatorRules()
    {
        return typeof(IPaymentTransactionValidator)
            .Assembly
            .GetTypes()
            .Where(px => px.IsClass && px.IsAssignableTo(typeof(IPaymentTransactionValidator)))
            .Select(Activator.CreateInstance)
            .Cast<IPaymentTransactionValidator>();
    }
}