using JetBrains.Annotations;
using SlackNet.Blocks;

namespace SlackNetBlockBuilder;

[PublicAPI]
public static class ButtonExtensions
{
    public static ActionElementBuilder<Button> Text(this ActionElementBuilder<Button> builder, string text)
        => builder.Set(x => x.Text = text);

    public static ActionElementBuilder<Button> Url(this ActionElementBuilder<Button> builder, string url)
    {
        builder.Element.Url = url;
        return builder;
    }

    public static ActionElementBuilder<Button> Value(this ActionElementBuilder<Button> builder, string value)
    {
        builder.Element.Value = value;
        return builder;
    }

    public static ActionElementBuilder<Button> Style(this ActionElementBuilder<Button> builder, ButtonStyle style)
    {
        builder.Element.Style = style;
        return builder;
    }

    public static ActionElementBuilder<Button> AccessibilityLabel(this ActionElementBuilder<Button> builder,
        string label)
    {
        builder.Element.AccessibilityLabel = label;
        return builder;
    }


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

    public static ActionsBlockBuilder AddButton(this ActionsBlockBuilder builder, string actionId, string text,
        string? url = null, string? value = null)
    {
        return builder.AddButton(actionId, text, ButtonStyle.Default, url, value);
    }
}