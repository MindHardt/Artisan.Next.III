using System.Reflection;
using System.Text.Json;
using ErrorOr;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Refit;

namespace Client.Infrastructure;

public static class BackendRefitConfiguration
{
    public static IHttpClientBuilder WithBackendPrefix(this IHttpClientBuilder clientBuilder) => clientBuilder
        .ConfigureHttpClient((sp, http) =>
        {
            http.BaseAddress = new Uri(sp.GetRequiredService<IWebAssemblyHostEnvironment>().BaseAddress);
        });
    
    public static RefitSettings GetSettings(IServiceProvider sp) => new()
    {
        ExceptionFactory = _ => Task.FromResult<Exception?>(null),
        ContentSerializer = new BackendContentSerializer(sp.GetRequiredService<IOptions<JsonSerializerOptions>>().Value)
    };

    private class BackendContentSerializer(JsonSerializerOptions jsonOptions) : IHttpContentSerializer
    {
        private readonly SystemTextJsonContentSerializer _innerSerializer = new(jsonOptions);


        public HttpContent ToHttpContent<T>(T item)
            => _innerSerializer.ToHttpContent(item);

        public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
            => _innerSerializer.GetFieldNameForProperty(propertyInfo);
        
        public async Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken ct = default)
        {
            if (typeof(T) == typeof(Error?))
            {
                return (T?)(object?)await GetPossibleError(content, ct);
            }

            // ReSharper disable once InvertIf
            if (typeof(T) is { IsGenericType: true, GenericTypeArguments: [var responseType] } genericType && 
                genericType.GetGenericTypeDefinition() == typeof(ErrorOr<>))
            {
                var method = typeof(BackendContentSerializer)
                    .GetMethod(nameof(GetErrorOr), BindingFlags.Static | BindingFlags.NonPublic)!
                    .MakeGenericMethod(responseType);

                var resultTask = (Task)method.Invoke(null, [content, ct])!;
                await resultTask;

                var result = typeof(Task<>).GetProperty(nameof(Task<bool>.Result))!.GetValue(resultTask);

                return (T?)result;
            }

            return await _innerSerializer.FromHttpContentAsync<T>(content, ct);
        }

        private static async Task<Error?> GetPossibleError(HttpContent content, CancellationToken ct)
        {
            var contentString = await content.ReadAsStringAsync(ct);
            return string.IsNullOrEmpty(contentString) is false
                ? GetError(JsonSerializer.Deserialize<ProblemDetails>(contentString)!)
                : null;
        }

        private static async Task<ErrorOr<T>> GetErrorOr<T>(HttpContent content, CancellationToken ct)
        {
            var contentString = await content.ReadAsStringAsync(ct);
            try
            {
                return GetError(JsonSerializer.Deserialize<ProblemDetails>(contentString)!);
            }
            catch
            {
                return JsonSerializer.Deserialize<T>(contentString)!;
            }
        }
        
        private static Error GetError(ProblemDetails problem)
        {
            var baseError = problem.Status switch
            {
                400 => Error.Validation(),
                401 => Error.Unauthorized(),
                403 => Error.Forbidden(),
                404 => Error.NotFound(),
                409 => Error.Conflict(),
                422 => Error.Unexpected(),
                _ => Error.Failure()
            };
            var description = problem.Detail ?? baseError.Description;
        
            return Error.Custom((int)baseError.Type, baseError.Code, description, baseError.Metadata);
        }
    }
}