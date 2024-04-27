using Microsoft.JSInterop;

namespace Client.Features.Shared;

public static class JsRuntimeExtensions
{
    /// <summary>
    /// The default javascript alert function that displays message in a floating window.
    /// </summary>
    /// <param name="js">The <see cref="IJSRuntime"/>.</param>
    /// <param name="message">The additional info displayed.</param>
    /// <returns></returns>
    public static ValueTask Alert(this IJSRuntime js, string message)
        => js.InvokeVoidAsync("alert", message);

    /// <summary>
    /// The default javascript confirm function that asks for user confirmation in a floating window.
    /// </summary>
    /// <param name="js">The <see cref="IJSRuntime"/>.</param>
    /// <param name="message">The additional info displayed.</param>
    /// <returns><see langword="true"/> if user agrees, otherwise <see langword="false"/>.</returns>
    public static ValueTask<bool> Confirm(this IJSRuntime js, string message)
        => js.InvokeAsync<bool>("confirm", message);

    /// <summary>
    /// The default javascript prompt function that asks for user input in a floating window.
    /// </summary>
    /// <param name="js">The <see cref="IJSRuntime"/>.</param>
    /// <param name="message">The additional info displayed.</param>
    /// <returns>A value provided by user.</returns>
    public static ValueTask<string> Prompt(this IJSRuntime js, string message)
        => js.InvokeAsync<string>("prompt", message);
}