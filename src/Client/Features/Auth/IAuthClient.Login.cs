using System.Web;
using Refit;

namespace Client.Features.Auth;

public partial interface IAuthClient
{
    public const string LoginPath = $"{Prefix}/login/{{{nameof(LoginRequest.Scheme)}}}";
    public string BuildLoginUrl(LoginRequest request)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.ReturnUrl)] = request.ReturnUrl;

        var baseUri = LoginPath.Replace($"{{{nameof(request.Scheme)}}}", request.Scheme);

        return $"{baseUri}?{query}";
    }
    public record LoginRequest(
        string Scheme,
        [Query] string ReturnUrl);
}