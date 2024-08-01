using ErrorOr;
using Refit;

namespace Client.Features.Auth;

public partial interface IAuthClient
{
    public const string UpdateProfilePath = $"{Prefix}/me";
    [Put(UpdateProfilePath)]
    public Task<Error?> UpdateProfile([Body] UpdateProfileRequest request, CancellationToken ct = default);
    public record UpdateProfileRequest(
        string? AvatarUrl,
        string? UserName);
}