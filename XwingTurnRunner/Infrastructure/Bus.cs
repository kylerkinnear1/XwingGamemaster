using System.Collections.Concurrent;

namespace XwingTurnRunner.Infrastructure;

public interface IRequest<out T> { }

public interface IBus
{
    Task Publish<TEvent>(TEvent message);
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
    
    void Subscribe<TEvent>(Func<TEvent, Task> handler);
    void Register<TRequest, TResponse>(Func<TRequest, Task<TResponse>> handler);
}

public class Bus : IBus
{
    private readonly ConcurrentDictionary<Type, Func<object, Task<object>>> _requestHandlers = new();
    private readonly ConcurrentDictionary<Type, List<Func<object, Task>>> _eventHandlers = new();

    public async Task Publish<TEvent>(TEvent message)
    { 
        var handlers = _eventHandlers.GetOrAdd(typeof(TEvent), _ => new());
        await Task.WhenAll(handlers.Select(handler => handler(message!)));
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        if (!_requestHandlers.TryGetValue(request.GetType(), out var handler))
        {
            throw new HandlerNotRegisteredException(request.GetType());
        }

        return (TResponse)await handler(request);
    }

    public void Subscribe<TEvent>(Func<TEvent, Task> handler)
    {
        var subscribed = _eventHandlers.GetOrAdd(typeof(TEvent), _ => new());
        subscribed.Add(evnt => handler((TEvent)evnt));
    }

    public void Register<TRequest, TResponse>(Func<TRequest, Task<TResponse>> handler)
    {
        if (!_requestHandlers.TryAdd(
                typeof(TRequest),
                x => handler((TRequest)x).ContinueWith(y => (object)y)))
        {
            throw new AlreadyRegisteredException(typeof(TRequest));
        }
    }
}

public class HandlerNotRegisteredException : Exception
{
    public HandlerNotRegisteredException(Type type) : base($"No handler registered for: {type.FullName}.")
    {
    }
}

public class AlreadyRegisteredException : Exception
{
    public AlreadyRegisteredException(Type type) : base($"Request Handler already registered for: {type.FullName}.")
    {
    }
}
