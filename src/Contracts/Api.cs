namespace Contracts;

public static class Api
{
    public const string Prefix = "/api";
}

public readonly record struct EmptyRequest;

public enum SortDirection
{
    Ascending,
    Descending
}