using System.Web;
using Refit;

namespace Client.Features.Auth;

public partial interface IAuthClient
{
    public const string LinkLoginPath = $"{Prefix}/link-login/{{{nameof(LinkLoginRequest.Scheme)}}}";
    public string BuildLinkLoginUrl(LinkLoginRequest request)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.ReturnUrl)] = request.ReturnUrl;

        var baseUri = LinkLoginPath.Replace($"{{{nameof(request.Scheme)}}}", request.Scheme);

        return $"{baseUri}?{query}";
    }
    public record LinkLoginRequest(
        string Scheme,
        [Query] string ReturnUrl);
}