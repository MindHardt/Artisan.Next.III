using System.Text.RegularExpressions;

namespace Client.Features.Wiki;

public partial class ManageBook
{
    [GeneratedRegex(@"\.(jpg|jpeg|png|webp)\s*\)")]
    private static partial Regex ImageRegex();
    
    private void UpdateText(string text) =>
        _text = ImageRegex().Replace(text, match =>
            $".{match.Groups[1]} =100%x*)");
}