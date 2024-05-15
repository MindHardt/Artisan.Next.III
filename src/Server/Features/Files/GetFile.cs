using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Data;
using CacheControlHeaderValue = Microsoft.Net.Http.Headers.CacheControlHeaderValue;

namespace Server.Features.Files;

[Handler]
[MapGet(Contracts.GetFile.FullPath)]
public partial class GetFile
{
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        (endpoint as RouteHandlerBuilder)?
        .Produces(StatusCodes.Status410Gone)
        .WithTags(nameof(FileEndpoints));

    [EndpointRegistrationOverride(nameof(AsParametersAttribute))]
    public record Request(
        [FromRoute] FileIdentifier Identifier,
        [FromQuery] Contracts.GetFile.Name? Name = null)
        : Contracts.GetFile.Request(Identifier, Name);
    
    private static async ValueTask<Results<PhysicalFileHttpResult, NotFound, StatusCodeHttpResult>> HandleAsync(
        // ReSharper disable once SuggestBaseTypeForParameter
        Request request,
        DataContext dataContext,
        IOptions<FileStorageOptions> fsOptions,
        HttpResponse response,
        CancellationToken ct)
    {
        var file = await dataContext.Files
            .FirstOrDefaultAsync(x => x.Identifier == request.Identifier, ct);
        if (file is null)
        {
            return TypedResults.NotFound();
        }
        
        response.Headers.CacheControl = CacheControlHeaderValue.PublicString;
        var fileName = request.Name switch
        {
            Contracts.GetFile.Name.Original => file.OriginalName,
            _ => file.Identifier.Value
        };

        var path = Path.Combine(fsOptions.Value.Directory, file.Hash.Value);
        if (File.Exists(path) is false)
        {
            return TypedResults.StatusCode(StatusCodes.Status410Gone);
        }
        
        return TypedResults.PhysicalFile(
            path,
            file.ContentType, 
            fileName, 
            file.CreatedAt);
    }
}