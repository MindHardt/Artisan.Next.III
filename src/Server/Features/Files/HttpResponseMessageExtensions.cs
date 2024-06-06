using System.Net.Mime;

namespace Server.Features.Files;

public static class HttpResponseMessageExtensions
{
    public static async Task<IFormFile> CreateFormFileAsync(this HttpResponseMessage response, CancellationToken ct = default)
    {
        var fileName = response.Content.Headers.ContentDisposition?.FileName
            ?? Path.GetFileName(response.RequestMessage?.RequestUri?.AbsolutePath)
            ?? Path.GetRandomFileName();

        return new FormFile(
            await response.Content.ReadAsStreamAsync(ct),
            0,
            response.Content.Headers.ContentLength ?? long.MaxValue,
            nameof(UploadFile.Request.File),
            fileName.Trim('"'))
        {
            Headers = new HeaderDictionary(),
            ContentType = response.Content.Headers.ContentType?.MediaType ?? MediaTypeNames.Application.Octet
        };
    }
}