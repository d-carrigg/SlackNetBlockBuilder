using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Provides extension methods for configuring <see cref="Button"/> elements and adding them to <see cref="ActionsBlock"/>.
/// </summary>
[PublicAPI]
public static class ButtonExtensions
{
    /// <summary>
    /// Sets the text displayed on the button.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="text">A plain_text object that defines the button's text. Maximum length is 75 characters.</param>
    /// <returns>The same instance so calls can be chained.</returns>
    public static ActionElementBuilder<Button> Text(this ActionElementBuilder<Button> builder, string text)
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            string.IsNullOrEmpty(text) ? throw new ArgumentNullException(nameof(text)) :
            builder.Set(x => x.Text = text);

    /// <summary>
    /// Sets the URL to open when the button is clicked.
    /// If a URL is provided, the button will not trigger an interaction payload (action_id is ignored).
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="url">A URL to load in the user's browser. Maximum length is 3000 characters.</param>
    /// <returns>The same instance so calls can be chained.</returns>
    public static ActionElementBuilder<Button> Url(this ActionElementBuilder<Button> builder, string url)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(url);
        
        builder.Element.Url = url;
        return builder;
    }
    
    /// <summary>
    /// Sets the URL to open when the button is clicked.
    /// If a URL is provided, the button will not trigger an interaction payload (action_id is ignored).
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="url">A URL to load in the user's browser. Maximum length is 3000 characters.</param>
    /// <returns>The same instance so calls can be chained.</returns>
    public static ActionElementBuilder<Button> Url(this ActionElementBuilder<Button> builder, Uri url)
   =>
       url is null ? throw new ArgumentNullException(nameof(url)) :
       builder.Url(url.ToString());

    /// <summary>
    /// Sets the value sent with the interaction payload when the button is clicked.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="value">The value to send. Maximum length is 2000 characters.</param>
    /// <returns>The same instance so calls can be chained.</returns>
    public static ActionElementBuilder<Button> Value(this ActionElementBuilder<Button> builder, string value)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Element.Value = value;
        return builder;
    }

    /// <summary>
    /// Sets the visual style of the button.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="style">The style to apply (Default, Primary, or Danger).</param>
    /// <returns>The same instance so calls can be chained.</returns>
    public static ActionElementBuilder<Button> Style(this ActionElementBuilder<Button> builder, ButtonStyle style)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Element.Style = style;
        return builder;
    }

    /// <summary>
    /// Sets a descriptive label for accessibility tools.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="label">The accessibility label. Used by screen readers. Maximum length is 75 characters.</param>
    /// <returns>The same instance so calls can be chained.</returns>
    public static ActionElementBuilder<Button> AccessibilityLabel(this ActionElementBuilder<Button> builder,
        string label)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Element.AccessibilityLabel = label;
        return builder;
    }


    /// <summary>
    /// Adds a button to an Actions block with the specified properties.
    /// </summary>
    /// <param name="builder">The ActionsBlockBuilder instance.</param>
    /// <param name="actionId">An identifier for this action. You can use this when you receive an interaction payload to identify the source of the action. Should be unique among all other action_ids used elsewhere by your app. Maximum length is 255 characters.</param>
    /// <param name="text">A plain_text object that defines the button's text. Maximum length is 75 characters.</param>
    /// <param name="style">The visual style of the button (Default, Primary, or Danger).</param>
    /// <param name="url">A URL to load in the user's browser. If provided, action_id is ignored. Max length 3000.</param>
    /// <param name="value">The value to send with the interaction payload. Max length 2000.</param>
    /// <returns>The same ActionsBlockBuilder instance so calls can be chained.</returns>
    public static ActionsBlockBuilder AddButton(this ActionsBlockBuilder builder, string actionId,
        string text,
        ButtonStyle style, string? url = null, string? value = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
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
    /// Adds a button to an Actions block with the specified properties.
    /// </summary>
    /// <param name="builder">The ActionsBlockBuilder instance.</param>
    /// <param name="actionId">An identifier for this action. You can use this when you receive an interaction payload to identify the source of the action. Should be unique among all other action_ids used elsewhere by your app. Maximum length is 255 characters.</param>
    /// <param name="text">A plain_text object that defines the button's text. Maximum length is 75 characters.</param>
    /// <param name="style">The visual style of the button (Default, Primary, or Danger).</param>
    /// <param name="url">A URL to load in the user's browser. If provided, action_id is ignored. Max length 3000.</param>
    /// <param name="value">The value to send with the interaction payload. Max length 2000.</param>
    /// <returns>The same ActionsBlockBuilder instance so calls can be chained.</returns>
    public static ActionsBlockBuilder AddButton(this ActionsBlockBuilder builder, string actionId,
        string text,
        ButtonStyle style, Uri? url = null, string? value = null)
    => url is null ? throw new ArgumentNullException(nameof(url)) :
        builder.AddButton(actionId, text, style, url.ToString(), value);
    
    
    /// <summary>
    /// Adds a button with default style to an Actions block.
    /// </summary>
    /// <param name="builder">The ActionsBlockBuilder instance.</param>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="text">A plain_text object that defines the button's text. Max length 75.</param>
    /// <param name="url">A URL to load in the user's browser. If provided, action_id is ignored. Max length 3000.</param>
    /// <param name="value">The value to send with the interaction payload. Max length 2000.</param>
    /// <returns>The same ActionsBlockBuilder instance so calls can be chained.</returns>
    public static ActionsBlockBuilder AddButton(this ActionsBlockBuilder builder, string actionId, string text,
        string? url = null, string? value = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(actionId);
        ArgumentNullException.ThrowIfNull(text);
        return builder.AddButton(actionId, text, ButtonStyle.Default, url, value);
    }
 
}