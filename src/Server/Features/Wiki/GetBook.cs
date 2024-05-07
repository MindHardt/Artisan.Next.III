using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Server.Features.Wiki;

[Handler]
[MapGet(Contracts.GetBook.FullPath)]
public partial class GetBook
{
    private static async ValueTask<Results<Ok<Contracts.GetBook.Response>, NotFound>> HandleAsync(
        Contracts.GetBook.Request request,
        DataContext dataContext,
        CancellationToken ct)
    {
        var book = await dataContext.Books
            .FirstOrDefaultAsync(x => x.UrlName == request.UrlName, ct);
        if (book is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new Contracts.GetBook.Response(
            book.UrlName,
            book.Name,
            book.Description,
            book.Author,
            book.Text,
            book.ImageUrl,
            book.IsPublic));
    }
}