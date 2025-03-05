using JetBrains.Annotations;
using SlackNet.Blocks;

namespace SlackNetBlockBuilder;

[PublicAPI]
public static class CheckboxGroupExtensions
{
    public static InputElementBuilder<CheckboxGroup> AddOption(this InputElementBuilder<CheckboxGroup> builder,
        string value,
        string text, PlainText description = null) => builder.Set(x =>
        x.Options.Add(new Option { Text = text, Value = value, Description = description }));

    public static InputElementBuilder<CheckboxGroup> FocusOnLoad(this InputElementBuilder<CheckboxGroup> builder,
        bool focus = true)
        => builder.Set(x => x.FocusOnLoad = focus);

    public static InputElementBuilder<CheckboxGroup> InitialOptions(this InputElementBuilder<CheckboxGroup> builder,
        Func<IList<Option>, IList<Option>> selector) => builder.Set(x => x.InitialOptions = selector(x.Options));

    public static InputElementBuilder<CheckboxGroup> InitialOptions(this InputElementBuilder<CheckboxGroup> builder,
        params string[] initialOptions) =>
        builder.InitialOptions(options => options.Where(o => initialOptions.Contains(o.Value)).ToList());
}