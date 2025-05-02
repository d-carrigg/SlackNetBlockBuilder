using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Extension methods for working with radio button group elements
/// </summary>
[PublicAPI]
public static class RadioButtonGroupExtensions
{
    /// <summary>
    /// Adds an option to the radio button group
    /// </summary>
    /// <param name="builder">The radio button group builder</param>
    /// <param name="value">The value to be sent to your app when the option is selected</param>
    /// <param name="text">The text to display for the option</param>
    /// <param name="description">Optional description for the option</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<RadioButtonGroup> AddOption(this InputElementBuilder<RadioButtonGroup> builder,
        string value,
        string text, PlainText description = null) => builder.Set(x =>
        x.Options.Add(new Option { Text = text, Value = value, Description = description }));

    /// <summary>
    /// Sets the initially selected option in the radio button group
    /// </summary>
    /// <param name="builder">The radio button group builder</param>
    /// <param name="value">The value of the option to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<RadioButtonGroup> InitialOption(
        this InputElementBuilder<RadioButtonGroup> builder,
        string value) => builder.Set(x => x.InitialOption = x.Options.FirstOrDefault(o => o.Value == value));

    /// <summary>
    /// Sets whether the radio button group should be focused when the view is opened
    /// </summary>
    /// <param name="builder">The radio button group builder</param>
    /// <param name="focus">Whether to focus the element</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<RadioButtonGroup> FocusOnLoad(
        this InputElementBuilder<RadioButtonGroup> builder,
        bool focus = true) => builder.Set(x => x.FocusOnLoad = focus);
}