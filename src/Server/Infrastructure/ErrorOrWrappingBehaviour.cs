using ErrorOr;
using Immediate.Handlers.Shared;

namespace Server.Infrastructure;

public class ErrorOrWrappingBehaviour<TRequest, TResponse> : Behavior<TRequest, TResponse>
{
    public override ValueTask<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return Next(request, cancellationToken);
        }
        catch (Exception e)
        {
            if (typeof(TResponse) == typeof(Error?))
            {
                return ValueTask.FromResult((TResponse)(object)Error.Failure(description: e.Message));
            }

            // ReSharper disable once InvertIf
            if (typeof(TResponse) is { GenericTypeArguments: [var responseType] } genericType &&
                genericType.GetGenericTypeDefinition() == typeof(ErrorOr<>))
            {
                List<Error> errors = [Error.Failure(description: e.Message)];
                var error = typeof(ErrorOr<>)
                    .GetMethod(nameof(ErrorOr<int>.From))!
                    .MakeGenericMethod(responseType)
                    .Invoke(null, parameters: [errors]);
                return ValueTask.FromResult((TResponse)error!);
            }

            throw;
        }
    }
}