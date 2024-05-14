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
        endpoint.RequireAuthorization(policy => policy.RequireRole(RoleNames.Admin)).WithTags(nameof(WikiEndpoints));
    
    [EndpointRegistrationOverride(nameof(AsParametersAttribute))]
    public record Request(
        [FromRoute] BookUrlName UrlName,
        [FromBody] Contracts.UpdateBook.Request Body);

    private static async ValueTask<Results<NotFound, Ok<BookModel>, ForbidHttpResult>> HandleAsync(
        Request request,
        DataContext dataContext,
        CancellationToken ct)
    {
        var book = await dataContext.Books
            .FirstOrDefaultAsync(x => x.UrlName == request.UrlName, ct);
        if (book is null)
        {
            return TypedResults.NotFound();
        }

        book.Author = request.Body.Author;
        book.Description = request.Body.Description;
        book.Text = request.Body.Text;
        book.ImageUrl = request.Body.ImageUrl;
        book.LastUpdated = DateTimeOffset.UtcNow;
        book.IsPublic = request.Body.IsPublic;
        
        await dataContext.SaveChangesAsync(ct);

        return TypedResults.Ok(new BookModel(
            book.UrlName,
            book.Name,
            book.Description,
            book.ImageUrl,
            book.Author,
            book.IsPublic));
    }

}