using JetBrains.Annotations;

namespace SlackNet.Blocks;

[PublicAPI]
public static class OverflowMenuExtensions
{
    public static ActionElementBuilder<OverflowMenu> AddOption(this ActionElementBuilder<OverflowMenu> builder,
        string value,
        string text,
        PlainText description = null,
        string url = null)
        => builder.Set(x =>
            x.Options.Add(new OverflowOption { Text = text, Value = value, Description = description, Url = url }));
}