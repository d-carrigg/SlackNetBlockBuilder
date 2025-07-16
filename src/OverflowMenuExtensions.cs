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
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static ActionElementBuilder<OverflowMenu> AddOption(this ActionElementBuilder<OverflowMenu> builder,
        string value,
        string text,
        PlainText? description = null,
        string? url = null)
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Modify(x =>
            x.Options.Add(new OverflowOption { Text = text, Value = value, Description = description, Url = url }));
}