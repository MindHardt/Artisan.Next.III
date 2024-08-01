using ErrorOr;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Server.Infrastructure;

public static class HttpStatusMappings
{
    public static Results<Ok, ProblemHttpResult> GetHttpResult(this Error? error) => error is not null
        ? TypedResults.Problem(GetProblemDetails(error.Value))
        : TypedResults.Ok();

    public static Results<Ok<T>, ProblemHttpResult> GetHttpResult<T>(this ErrorOr<T> errorOr) => errorOr.IsError
        ? TypedResults.Problem(GetProblemDetails(errorOr.FirstError))
        : TypedResults.Ok(errorOr.Value);

    public static ProblemDetails GetProblemDetails(Error error) => new()
    {
        Detail = error.Description,
        Status = error.Type switch
        {
            ErrorType.Validation => 400,
            ErrorType.Unauthorized => 401,
            ErrorType.Forbidden => 403,
            ErrorType.NotFound => 404,
            ErrorType.Conflict => 409,
            ErrorType.Unexpected => 422,
            _ => 500
        }
    };
}