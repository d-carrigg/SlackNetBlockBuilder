using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Extension methods for working with checkbox group elements
/// </summary>
[PublicAPI]
public static class CheckboxGroupExtensions
{
    /// <summary>
    /// Adds an option to the checkbox group
    /// </summary>
    /// <param name="builder">The checkbox group builder</param>
    /// <param name="value">The value to be sent to your app when the option is selected</param>
    /// <param name="text">The text to display for the option</param>
    /// <param name="description">Optional description for the option</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<CheckboxGroup> AddOption(this InputElementBuilder<CheckboxGroup> builder,
        string value,
        string text, PlainText? description = null) => builder.Set(x =>
        x.Options.Add(new Option { Text = text, Value = value, Description = description }));

    /// <summary>
    /// Sets whether the checkbox group should be focused when the view is opened
    /// </summary>
    /// <param name="builder">The checkbox group builder</param>
    /// <param name="focus">Whether to focus the element</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<CheckboxGroup> FocusOnLoad(this InputElementBuilder<CheckboxGroup> builder,
        bool focus = true)
        => builder.Set(x => x.FocusOnLoad = focus);

    /// <summary>
    /// Sets the initially selected options in the checkbox group
    /// </summary>
    /// <param name="builder">The checkbox group builder</param>
    /// <param name="selector">A function that selects which options should be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<CheckboxGroup> InitialOptions(this InputElementBuilder<CheckboxGroup> builder,
        Func<IList<Option>, IList<Option>> selector) => builder.Set(x => x.InitialOptions = selector(x.Options));

    /// <summary>
    /// Sets the initially selected options in the checkbox group by their values
    /// </summary>
    /// <param name="builder">The checkbox group builder</param>
    /// <param name="initialOptions">The values of the options to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<CheckboxGroup> InitialOptions(this InputElementBuilder<CheckboxGroup> builder,
        params string[] initialOptions) =>
        builder.InitialOptions(options => options.Where(o => initialOptions.Contains(o.Value)).ToList());
}