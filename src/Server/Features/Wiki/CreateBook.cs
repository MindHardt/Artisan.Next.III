using System.Security.Claims;
using Client.Features.Auth;
using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NickBuhro.Translit;
using Server.Data;
using Vogen;

namespace Server.Features.Wiki;

[Handler]
[MapPost(Contracts.CreateBook.FullPath)]
public partial class CreateBook
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.RequireAuthorization(policy => policy.RequireRole(RoleNames.Writer)).WithTags(nameof(WikiEndpoints));

    private static async ValueTask<Results<Ok<BookModel>, Conflict, ProblemHttpResult>> HandleAsync(
        Contracts.CreateBook.Request request,
        ClaimsPrincipal principal,
        DataContext dataContext,
        CancellationToken ct)
    {
        var urlName = CreateUrlName(request.Name);
        if (urlName is null)
        {
            return TypedResults.Problem($"Failed to create {nameof(BookUrlName)} from name '{request.Name}'");
        }

        var urlNameTaken = await dataContext.Books
            .AnyAsync(x => x.UrlName == urlName, ct);
        if (urlNameTaken)
        {
            return TypedResults.Conflict();
        }

        var userId = principal.GetRequiredUserId();
        var book = new Book
        {
            UrlName = urlName.Value,
            Name = request.Name,
            Description = request.Description,
            Author = request.Author,
            ImageUrl = request.ImageUrl,
            Text = request.Text,
            LastUpdated = DateTimeOffset.UtcNow,
            IsPublic = request.IsPublic,
            OwnerId = userId
        };
        dataContext.Books.Add(book);
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
    
    private static BookUrlName? CreateUrlName(string name)
    {
        var rawValue = new string(
            Transliteration.CyrillicToLatin(name, Language.Russian)
                .Replace(' ', '-')
                .ToLower()
                .Where(x => x is >= 'a' and <= 'z' or >= '0' and <= '9' or '-')
                .ToArray());

        return BookUrlName.TryFrom(rawValue, out var result) ? result : null;
    }
}