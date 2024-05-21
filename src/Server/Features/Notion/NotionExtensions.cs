using Microsoft.AspNetCore.Components;
using Notion.Client;

namespace Server.Features.Notion;

public static class NotionExtensions
{
    public static TitlePropertyValue GetTitle(this Page page) => page.Properties
        .Values
        .OfType<TitlePropertyValue>()
        .Single();

    public static string ToPlainText(this IEnumerable<RichTextBase> richTexts) => string.Concat(richTexts
        .Select(rt => rt switch
        {
            _ => rt.PlainText
        }));

    public static MarkupString ToHtml(this IEnumerable<RichTextBase> richTexts) => (MarkupString)string.Concat(richTexts
        .Select(rt => rt switch
        {
            RichTextMention mention => $"<a href=\"{mention.Href}\">{mention.PlainText}</a>",
            _ => rt.PlainText
        }));
}