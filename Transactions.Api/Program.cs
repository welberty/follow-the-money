using Foundation.Business.Data;
using Foundation.Business.DomainNotitications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Transactions.Business.Contracts;
using Transactions.Business.UseCase;
using Transactions.Data.Context;
using Transactions.Data.Repository;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Foundation.Business.BehaviorHandlers;
using Extensions.OpenTelemetry;
using Foundation.Settings;
using Extensions.Log;
using Extensions.Authentication;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", AutenticationExtension.OpenApiSecurityScheme);
    o.AddSecurityRequirement(AutenticationExtension.OpenApiSecurityRequirement);
});

var appSettings = new AppSettings();
builder.Configuration.Bind(appSettings);

builder.AddSerilog();

builder.Services
            .AddAuthentication(appSettings)
            .AddOpenTelemetry(appSettings)
            .AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("rabbitmq2", "/", h => {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(ctx);
                });
            })
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(TransactionInputDto).Assembly, typeof(AddTransactionUseCase).Assembly);
            })
            .AddScoped<DomainNotificationContext, TransactionDomainNotificationContext>()
            .AddScoped<ITransactionRepository, TransactionRepository>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddDbContext<Transactions.Data.Context.TransactionContext>(opt => opt.UseInMemoryDatabase("InMemoryTransactionDb"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapLogin(appSettings);

app.MapPost("/transaction/add", [Authorize]  async (
    [FromBody] TransactionInputDto input,
    IMediator mediator,
    DomainNotificationContext domainNotificaionContext,
    CancellationToken cancellationToken) => {
        await mediator.Send(input, cancellationToken);
        if (!domainNotificaionContext.IsValid)
        {
            return Results.BadRequest(domainNotificaionContext.Notifications);
        }
        return Results.Created("/", null);
    })
.WithOpenApi();

app.Run();

