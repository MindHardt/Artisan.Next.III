using System.Text.Json.Serialization;
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
[MapPut(IWikiClient.UpdateBookPath)]
public partial class UpdateBook
{
    internal static Results<Ok<BookModel>, ProblemHttpResult> TransformResult(
        ErrorOr<BookModel> value) => value.GetHttpResult();
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.RequireAuthorization(policy => policy.RequireRole(RoleNames.Admin)).WithTags(nameof(IWikiClient));
    
    private static async ValueTask<ErrorOr<BookModel>> HandleAsync(
        [AsParameters] IWikiClient.UpdateBookRequest request,
        DataContext dataContext,
        CancellationToken ct)
    {
        var book = await dataContext.Books
            .FirstOrDefaultAsync(x => x.UrlName == request.UrlName, ct);
        if (book is null)
        {
            return Error.NotFound($"Book {request.UrlName} not found.");
        }

        book.Author = request.Author;
        book.Description = request.Description;
        book.Text = request.Text;
        book.ImageUrl = request.ImageUrl;
        book.LastUpdated = DateTimeOffset.UtcNow;
        book.IsPublic = request.IsPublic;

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