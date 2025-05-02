using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Extension methods for working with overflow menu elements
/// </summary>
[PublicAPI]
public static class OverflowMenuExtensions
{
    /// <summary>
    /// Adds an option to the overflow menu
    /// </summary>
    /// <param name="builder">The overflow menu builder</param>
    /// <param name="value">The value to be sent to your app when the option is selected</param>
    /// <param name="text">The text to display for the option</param>
    /// <param name="description">Optional description for the option</param>
    /// <param name="url">Optional URL to open when the option is selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static ActionElementBuilder<OverflowMenu> AddOption(this ActionElementBuilder<OverflowMenu> builder,
        string value,
        string text,
        PlainText description = null,
        string url = null)
        => builder.Set(x =>
            x.Options.Add(new OverflowOption { Text = text, Value = value, Description = description, Url = url }));
}