using System.Web;
using Refit;

namespace Client.Features.Auth;

public partial interface IAuthClient
{
    public const string LogoutPath = $"{Prefix}/logout";
    public string BuildLogoutUrl(LogoutRequest request)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.ReturnUrl)] = request.ReturnUrl;

        return $"{LogoutPath}?{query}";
    }
    public record LogoutRequest(
        [Query] string ReturnUrl);    
}