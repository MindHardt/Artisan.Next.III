namespace Contracts;

public static class RoleNames
{
    public const string Admin = "Admin";


    public static IEnumerable<string> Enumerate() => typeof(RoleNames)
        .GetFields()
        .Select(x => x.GetValue(null))
        .Cast<string>();
}