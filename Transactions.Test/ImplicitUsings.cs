global using FluentAssertions;
global using TechTalk.SpecFlow;
using Foundation.Business.Data;
using Foundation.Business.DomainNotitications;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolidToken.SpecFlow.DependencyInjection;
using Transactions.Business.Contracts;
using Transactions.Business.UseCase;
using Transactions.Data.Context;
using Transactions.Data.Repository;
using Transactions.Test;

public static class TestDependencies
{
    [ScenarioDependencies]
    public static IServiceCollection CreateServices()
    {
        var services = new ServiceCollection();
        services
            .AddLogging()
            .AddScoped<DomainNotificationContext, TestDomainNotificationContext>()
            .AddScoped<ITransactionRepository, TransactionRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddDbContext<Transactions.Data.Context.TransactionContext>(opt => opt.UseInMemoryDatabase("InMemoryTransactionDb"));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(TransactionInputDto).Assembly, typeof(AddTransactionUseCase).Assembly);

        });

        services.AddMassTransit(x =>
        {
            x.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
        // TODO: add your test dependencies here

        return services;
    }

}


