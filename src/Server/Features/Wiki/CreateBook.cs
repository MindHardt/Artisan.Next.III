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
        endpoint.RequireAuthorization(policy => policy.RequireRole(RoleNames.Admin)).WithTags(nameof(WikiEndpoints));

    private static async ValueTask<Results<Ok<BookModel>, Conflict, ProblemHttpResult>> HandleAsync(
        Contracts.CreateBook.Request request,
        DataContext dataContext,
        CancellationToken ct)
    {
        BookUrlName urlName;
        try
        {
            urlName = CreateUrlName(request.Name);
        }
        catch (ValueObjectValidationException)
        {
            return TypedResults.Problem($"Failed to create {nameof(BookUrlName)} from name '{request.Name}'");
        }

        var urlNameTaken = await dataContext.Books
            .AnyAsync(x => x.UrlName == urlName, ct);
        if (urlNameTaken)
        {
            return TypedResults.Conflict();
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

        return TypedResults.Ok(new BookModel(
            book.UrlName,
            book.Name,
            book.Description,
            book.ImageUrl,
            book.Author,
            book.IsPublic));
    }

    private static BookUrlName CreateUrlName(string name)
    {
        var rawValue = new string(
            Transliteration.CyrillicToLatin(name, Language.Russian)
            .Replace(' ', '-')
            .ToLower()
            .Where(x => x is >= 'a' and <= 'z' or >= '0' and <= '9' or '-')
            .ToArray());

        return BookUrlName.From(rawValue);
    }
}