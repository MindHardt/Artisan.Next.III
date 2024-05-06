using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ErrorOr;

namespace Client.Features.Shared;

public static class ErrorOrExtensions
{
    public static Error? AsPossibleError(this HttpResponseMessage response) => response.StatusCode switch
    {
        HttpStatusCode.NotFound => Error.NotFound(),
        HttpStatusCode.Unauthorized => Error.Unauthorized(),
        HttpStatusCode.Forbidden => Error.Forbidden(),
        HttpStatusCode.Conflict => Error.Conflict(),
        HttpStatusCode.BadRequest => Error.Validation(),

        HttpStatusCode.OK => null,
        _ => Error.Unexpected()
    };

    public static async Task<Error?> AsPossibleError(
        this Task<HttpResponseMessage> responseTask) 
        => (await responseTask).AsPossibleError();
    
    public static async Task<ErrorOr<T>> AsErrorOr<T>(
        this HttpResponseMessage response,
        JsonSerializerOptions jsonOptions,
        CancellationToken ct = default) 
        => response.AsPossibleError() ?? 
           await response.Content.ReadFromJsonAsync<T>(jsonOptions, ct) ??
           (ErrorOr<T>)Error.Unexpected();

    public static async Task<ErrorOr<T>> AsErrorOr<T>(
        this Task<HttpResponseMessage> task,
        JsonSerializerOptions jsonOptions,
        CancellationToken ct = default) 
        => await (await task).AsErrorOr<T>(jsonOptions, ct);
}