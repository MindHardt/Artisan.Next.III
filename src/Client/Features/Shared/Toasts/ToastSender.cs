using System.Diagnostics;

namespace Client.Features.Shared.Toasts;
#pragma warning disable CA2254

[RegisterScoped]
public class ToastSender(ILogger<ToastSender> logger)
{
    public event Func<ToastMessage, Task> ToastShown = toast =>
    {
        LogMessage(logger, toast);
        return Task.CompletedTask;
    };

    public async Task ShowToast(ToastMessage message)
        => await ToastShown.Invoke(message);

    public Task ShowInfoToast(string message)
        => ShowToast(new ToastMessage(ToastType.Info, message));

    public Task ShowWarningToast(string message)
        => ShowToast(new ToastMessage(ToastType.Warning, message));

    public Task ShowErrorToast(string message)
        => ShowToast(new ToastMessage(ToastType.Error, message));

    private static void LogMessage(ILogger logger, ToastMessage message)
    {
        var level = message.Type switch
        {
            ToastType.Info => LogLevel.Information,
            ToastType.Warning => LogLevel.Warning,
            ToastType.Error => LogLevel.Error,
            _ => throw new UnreachableException()
        };
        logger.Log(level, message.Text);
    }
}

public readonly record struct ToastMessage(ToastType Type, string Text);

public enum ToastType : byte
{
    Info,
    Warning,
    Error,
}

#pragma warning disable CA2254