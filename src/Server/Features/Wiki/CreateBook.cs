using Client.Features.Auth;
using Client.Features.Wiki;
using Client.Features.Wiki.Books;
using ErrorOr;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Infrastructure;

namespace Server.Features.Wiki;

[Handler]
[MapPost(IWikiClient.CreateBookPath)]
public partial class CreateBook
{
    internal static Results<Ok<BookModel>, ProblemHttpResult> TransformResult(
        ErrorOr<BookModel> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.RequireAuthorization(policy => policy.RequireRole(RoleNames.Admin)).WithTags(nameof(IWikiClient));

    private static async ValueTask<ErrorOr<BookModel>> HandleAsync(
        [FromBody] IWikiClient.CreateBookRequest request,
        DataContext dataContext,
        CancellationToken ct)
    {
        if (BookUrlName.TryFrom(request.Name, out var urlName) is false)
        {
            return Error.Validation($"Name {request.Name} could not be transformed to book url name");
        }

        var urlNameTaken = await dataContext.Books
            .AnyAsync(x => x.UrlName == urlName, ct);
        if (urlNameTaken)
        {
            return Error.Conflict($"Url name {urlName} is already taken!");
        }

        var book = new Book
        {
            UrlName = urlName,
            Name = request.Name,
            Description = request.Description,
            Author = request.Author,
            ImageUrl = request.ImageUrl,
            Text = request.Text,
            LastUpdated = DateTimeOffset.UtcNow,
            IsPublic = request.IsPublic
        };
        dataContext.Books.Add(book);
        await dataContext.SaveChangesAsync(ct);

        return new BookModel(
            book.UrlName,
            book.Name,
            book.Description,
            book.ImageUrl,
            book.Author,
            book.IsPublic);
    }
}