using System.Web;

namespace Contracts;

public interface IAuthClient
{
    public Task<LoginSchemeModel[]> GetLoginSchemes(CancellationToken ct = default);

    public Task UpdateProfile(UpdateProfile.Request request, CancellationToken ct = default);

    public string BuildLoginUrl(Login.Request request)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.ReturnUrl)] = request.ReturnUrl;

        var baseUri = $"{Login.FullPath}".Replace($"{{{nameof(request.Scheme)}}}", request.Scheme);

        return $"{baseUri}?{query}";
    }

    public string BuildLinkLoginUrl(LinkLogin.Request request)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.ReturnUrl)] = request.ReturnUrl;

        var baseUri = $"{LinkLogin.FullPath}".Replace($"{{{nameof(request.Scheme)}}}", request.Scheme);

        return $"{baseUri}?{query}";
    }

    public string BuildLogoutUrl(Logout.Request request)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query[nameof(request.ReturnUrl)] = request.ReturnUrl;

        return $"{Logout.FullPath}?{query}";
    }
}