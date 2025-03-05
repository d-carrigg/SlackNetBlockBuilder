using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Extension methods for working with button elements
/// </summary>
[PublicAPI]
public static class ButtonExtensions
{
    /// <summary>
    /// Sets the text for the button
    /// </summary>
    /// <param name="builder">The button builder</param>
    /// <param name="text">The text to display on the button</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static ActionElementBuilder<Button> Text(this ActionElementBuilder<Button> builder, string text)
        => builder.Set(x => x.Text = text);

    /// <summary>
    /// Sets the URL to open when the button is clicked
    /// </summary>
    /// <param name="builder">The button builder</param>
    /// <param name="url">The URL to open</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static ActionElementBuilder<Button> Url(this ActionElementBuilder<Button> builder, string url)
    {
        builder.Element.Url = url;
        return builder;
    }

    /// <summary>
    /// Sets the value to be sent to your app when the button is clicked
    /// </summary>
    /// <param name="builder">The button builder</param>
    /// <param name="value">The value to send</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static ActionElementBuilder<Button> Value(this ActionElementBuilder<Button> builder, string value)
    {
        builder.Element.Value = value;
        return builder;
    }

    /// <summary>
    /// Sets the visual style of the button
    /// </summary>
    /// <param name="builder">The button builder</param>
    /// <param name="style">The style to apply to the button</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static ActionElementBuilder<Button> Style(this ActionElementBuilder<Button> builder, ButtonStyle style)
    {
        builder.Element.Style = style;
        return builder;
    }

    /// <summary>
    /// Sets the accessibility label for the button
    /// </summary>
    /// <param name="builder">The button builder</param>
    /// <param name="label">The accessibility label</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static ActionElementBuilder<Button> AccessibilityLabel(this ActionElementBuilder<Button> builder,
        string label)
    {
        builder.Element.AccessibilityLabel = label;
        return builder;
    }

    /// <summary>
    /// Adds a button to the actions block with the specified style
    /// </summary>
    /// <param name="builder">The actions block builder</param>
    /// <param name="actionId">The action ID for the button</param>
    /// <param name="text">The text to display on the button</param>
    /// <param name="style">The style to apply to the button</param>
    /// <param name="url">Optional URL to open when the button is clicked</param>
    /// <param name="value">Optional value to send to your app when the button is clicked</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static ActionsBlockBuilder AddButton(this ActionsBlockBuilder builder, string actionId,
        string text,
        ButtonStyle style, string? url = null, string? value = null)
    {
        return builder.AddButton(actionId, button =>
            {
                button.Element.ActionId = actionId;
                button.Element.Text = text;
                button.Element.Url = url;
                button.Element.Style = style;
                button.Element.Value = value;
            });
    }

    /// <summary>
    /// Adds a button with default style to the actions block
    /// </summary>
    /// <param name="builder">The actions block builder</param>
    /// <param name="actionId">The action ID for the button</param>
    /// <param name="text">The text to display on the button</param>
    /// <param name="url">Optional URL to open when the button is clicked</param>
    /// <param name="value">Optional value to send to your app when the button is clicked</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static ActionsBlockBuilder AddButton(this ActionsBlockBuilder builder, string actionId, string text,
        string? url = null, string? value = null)
    {
        return builder.AddButton(actionId, text, ButtonStyle.Default, url, value);
    }
}