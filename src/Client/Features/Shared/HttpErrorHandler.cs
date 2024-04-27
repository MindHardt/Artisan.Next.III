using Client.Features.Shared.Toasts;

namespace Client.Features.Shared;

public class HttpErrorHandler(ToastSender toastSender) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var result = await base.SendAsync(request, cancellationToken);
        if (result.IsSuccessStatusCode is false)
        {
            await toastSender.ShowWarningToast(
                $"Запрос вернул ошибку {(int)result.StatusCode} {result.StatusCode}");
        }

        return result;
    }
}