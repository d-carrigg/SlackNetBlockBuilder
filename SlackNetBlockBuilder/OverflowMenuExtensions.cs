using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Provides extension methods for configuring <see cref="OverflowMenu"/> elements within an <see cref="ActionElementBuilder{TElement}"/>.
/// </summary>
[PublicAPI]
public static class OverflowMenuExtensions
{
    /// <summary>
    /// Adds an option to the overflow menu.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="value">The string value that will be passed to your app when this option is chosen. Maximum length 75 characters.</param>
    /// <param name="text">A plain text object that defines the text shown in the option on the menu. Maximum length 75 characters.</param>
    /// <param name="description">An optional plain text object shown below the <paramref name="text"/> field. Maximum length 75 characters.</param>
    /// <param name="url">An optional URL to navigate to when this option is clicked. Maximum length 3000 characters.</param>
    /// <summary>
            /// Adds an option to the overflow menu in the builder.
            /// </summary>
            /// <param name="builder">The builder to extend.</param>
            /// <param name="value">The value sent to the app when this option is selected (max 75 characters).</param>
            /// <param name="text">The visible text for the option (max 75 characters).</param>
            /// <param name="description">Optional secondary text shown below the main text (max 75 characters).</param>
            /// <param name="url">Optional URL to open when the option is clicked (max 3000 characters).</param>
            /// <returns>The same builder instance for method chaining.</returns>
    public static ActionElementBuilder<OverflowMenu> AddOption(this ActionElementBuilder<OverflowMenu> builder,
        string value,
        string text,
        PlainText? description = null,
        string? url = null)
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Set(x =>
            x.Options.Add(new OverflowOption { Text = text, Value = value, Description = description, Url = url }));
}