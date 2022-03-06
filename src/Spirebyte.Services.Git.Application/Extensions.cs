using Convey;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using Microsoft.Extensions.DependencyInjection;
using Spirebyte.Services.Git.Application.Git.Services;
using Spirebyte.Services.Git.Application.Git.Services.Interfaces;

namespace Spirebyte.Services.Git.Application;

public static class Extensions
{
    public static IConveyBuilder AddApplication(this IConveyBuilder builder)
    {
        builder.Services.AddSingleton<IRepositoryService, RepositoryService>();

        return builder
            .AddCommandHandlers()
            .AddEventHandlers()
            .AddInMemoryCommandDispatcher()
            .AddInMemoryEventDispatcher();
    }
}