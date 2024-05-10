using System.Text.RegularExpressions;

namespace Client.Features.Wiki.Books;

public partial class ReadBookPage
{
    private string GetText() => _includeImages
        ? _book!.Text
        : ExcludeImageRegex().Replace(_book!.Text, string.Empty);
    
    [GeneratedRegex(@"!\[.*?\]\(.*?\)")]
    private static partial Regex ExcludeImageRegex();
}