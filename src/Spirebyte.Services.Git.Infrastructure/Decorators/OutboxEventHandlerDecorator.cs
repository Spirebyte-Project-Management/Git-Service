using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Convey.MessageBrokers;
using Convey.MessageBrokers.Outbox;
using Convey.Types;

namespace Spirebyte.Services.Git.Infrastructure.Decorators;

[Decorator]
internal sealed class OutboxEventHandlerDecorator<TEvent> : IEventHandler<TEvent>
    where TEvent : class, IEvent
{
    private readonly bool _enabled;
    private readonly IEventHandler<TEvent> _handler;
    private readonly string _messageId;
    private readonly IMessageOutbox _outbox;

    public OutboxEventHandlerDecorator(IEventHandler<TEvent> handler, IMessageOutbox outbox,
        OutboxOptions outboxOptions, IMessagePropertiesAccessor messagePropertiesAccessor)
    {
        _handler = handler;
        _outbox = outbox;
        _enabled = outboxOptions.Enabled;
        _messageId = messagePropertiesAccessor.MessageProperties?.MessageId;
    }

    public Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default)
    {
        return _enabled && !string.IsNullOrWhiteSpace(_messageId)
            ? _outbox.HandleAsync(_messageId, () => _handler.HandleAsync(@event, cancellationToken))
            : _handler.HandleAsync(@event, cancellationToken);
    }
}