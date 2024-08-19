using System.Security.Claims;
using Client.Features.Auth;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Features.Wiki;

[Handler]
[MapPut(Contracts.UpdateBook.FullPath)]
public partial class UpdateBook
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.RequireAuthorization(policy => policy.RequireRole(RoleNames.Writer)).WithTags(nameof(WikiEndpoints));

    public record Request(
        [FromRoute] BookUrlName UrlName,
        [FromBody] Contracts.UpdateBook.Request Body);

    private static async ValueTask<Results<NotFound, Ok<BookModel>, ForbidHttpResult>> HandleAsync(
        [AsParameters] Request request,
        ClaimsPrincipal principal,
        DataContext dataContext,
        CancellationToken ct)
    {
        var book = await dataContext.Books
            .FirstOrDefaultAsync(x => x.UrlName == request.UrlName, ct);
        if (book is null)
        {
            return TypedResults.NotFound();
        }

        var userId = principal.GetRequiredUserId();
        var isEditedByOwner = book.OwnerId == userId;
        var isEditedByAdmin = principal.IsInRole(RoleNames.Admin);
        if (isEditedByOwner is false && isEditedByAdmin is false)
        {
            return TypedResults.Forbid();
        }

        book.Author = request.Body.Author;
        book.Description = request.Body.Description;
        book.Text = request.Body.Text;
        book.ImageUrl = request.Body.ImageUrl;
        book.LastUpdated = DateTimeOffset.UtcNow;
        book.IsPublic = request.Body.IsPublic;

        await dataContext.SaveChangesAsync(ct);

        var editable = book.OwnerId == userId || principal.IsInRole(RoleNames.Admin);
        return TypedResults.Ok(new BookModel(
            book.UrlName,
            book.Name,
            book.Description,
            book.ImageUrl,
            book.Author,
            book.IsPublic,
            editable));
    }

}