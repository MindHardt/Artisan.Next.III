using System.Text.Json;
using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Server.Features.Shared;

public static class ErrorOrExtensions
{
    public static Error? AsPossibleError(this IResult result) => result switch
    {
        NotFound => Error.NotFound(),
        UnauthorizedHttpResult => Error.Unauthorized(),
        ForbidHttpResult => Error.Forbidden(),
        Conflict => Error.Conflict(),
        BadRequest => Error.Validation(),

        Ok => null,
        _ => Error.Unexpected()
    };

    public static async Task<Error?> AsPossibleError(this ValueTask<IResult> resultTask)
        => (await resultTask).AsPossibleError();

    public static ErrorOr<T> AsErrorOr<T>(this IResult result) => result switch
        {
            Ok<T> { Value: { } value } => value,
            INestedHttpResult { Result: { } innerResult } => innerResult.AsErrorOr<T>(),
            _ => result.AsPossibleError() ?? Error.Unexpected()
        };

    public static async Task<ErrorOr<T>> AsErrorOr<T>(
        this ValueTask<IResult> resultTask)
        => (await resultTask).AsErrorOr<T>();

}