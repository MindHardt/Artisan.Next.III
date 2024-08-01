using System.Reflection;

namespace Client.Features.Auth;

public static class RoleNames
{
    public const string Admin = "Admin";


    public static IEnumerable<string> Enumerate() => typeof(RoleNames)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(x => x.GetValue(null))
        .OfType<string>();
}