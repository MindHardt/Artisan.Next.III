namespace Contracts;

public static class RoleNames
{
    public const string Admin = "Admin";
    public const string Writer = "Writer";

    public static IReadOnlyCollection<string> Collection { get; } = typeof(RoleNames)
        .GetFields()
        .Select(x => x.GetValue(null))
        .Cast<string>()
        .ToArray();

    public static IEnumerable<string> Enumerate() => Collection;
}