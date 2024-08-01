using Microsoft.AspNetCore.Components.Routing;

namespace Client.Features.Shared.Layout;

public readonly record struct NavLinkInfo(
    string Label,
    string Href,
    string Icon,
    NavLinkMatch Match = NavLinkMatch.Prefix);