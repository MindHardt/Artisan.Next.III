using Contracts;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Server.Features.Files;

[Handler]
[MapGet(Contracts.GetBlogposts.FullPath)]
public partial class GetBlogposts
{
    private static readonly string RelativePath = Path.Combine("static", "blogposts");
    internal static void CustomizeEndpoint(IEndpointConventionBuilder endpoint) =>
        endpoint.WithTags(nameof(FileEndpoints));

    private static ValueTask<Ok<IReadOnlyCollection<FileIdentifier>>> HandleAsync(
        EmptyRequest _,
        IWebHostEnvironment hostEnvironment,
        CancellationToken ct = default)
    {
        var blogpostDir = new DirectoryInfo(Path.Combine(hostEnvironment.WebRootPath, RelativePath));
        var blogposts = blogpostDir
            .GetFiles()
            .OrderByDescending(x => x.Name)
            .Select(x => FileIdentifier.From(Path.Combine(RelativePath, x.Name)))
            .ToArray();
        
        return ValueTask.FromResult(TypedResults.Ok(blogposts as IReadOnlyCollection<FileIdentifier>));
    }
    
}