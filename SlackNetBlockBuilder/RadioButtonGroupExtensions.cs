using JetBrains.Annotations;

namespace SlackNet.Blocks;

[PublicAPI]
public static class RadioButtonGroupExtensions
{
    public static InputElementBuilder<RadioButtonGroup> AddOption(this InputElementBuilder<RadioButtonGroup> builder,
        string value,
        string text, PlainText description = null) => builder.Set(x =>
        x.Options.Add(new Option { Text = text, Value = value, Description = description }));

    public static InputElementBuilder<RadioButtonGroup> InitialOption(
        this InputElementBuilder<RadioButtonGroup> builder,
        string value) => builder.Set(x => x.InitialOption = x.Options.FirstOrDefault(o => o.Value == value));

    public static InputElementBuilder<RadioButtonGroup> FocusOnLoad(
        this InputElementBuilder<RadioButtonGroup> builder,
        bool focus = true) => builder.Set(x => x.FocusOnLoad = focus);
}