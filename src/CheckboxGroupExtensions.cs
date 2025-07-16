using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="CheckboxGroup"/>. This class is intended to be used as a part
/// of a <see cref="BlockBuilder"/>.
/// </summary>
[PublicAPI]
public static class CheckboxGroupExtensions
{
    /// <summary>
    /// Add an option to a checkbox group. 
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="value">A text object that defines the text shown in the option on the menu. Overflow, select, and multi-select menus can only use plain_text objects, while radio buttons and checkboxes can use mrkdwn text objects. Maximum length for the text in this field is 75 characters.</param>
    /// <param name="text">A unique string value that will be passed to your app when this option is chosen. Maximum length for this field is 150 characters.</param>
    /// <param name="description">A plain_text text object that defines a line of descriptive text shown below the text field beside a single selectable item in a select menu, multi-select menu, checkbox group, radio button group, or overflow menu. Checkbox group and radio button group items can also use mrkdwn formatting. Maximum length for the text within this field is 75 characters.</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<CheckboxGroup> AddOption(this InputElementBuilder<CheckboxGroup> builder,
        string value,
        string text, PlainText? description = null) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Modify(x =>
        x.Options.Add(new Option { Text = text, Value = value, Description = description }));

    /// <summary>
    /// Indicates whether the element will be set to auto focus within the view object. Only one element can be set to true. Defaults to false.
    /// </summary>
    /// <param name="builder">The builder instance</param>
    /// <param name="focus">If true, the element will be focused when the view is opened. Defaults to false.</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<CheckboxGroup> FocusOnLoad(this InputElementBuilder<CheckboxGroup> builder,
        bool focus = true)
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Modify(x => x.FocusOnLoad = focus);

    /// <summary>
    /// Pre-selects options in the checkbox group.
    /// Takes a function that receives the current list of options and returns the list of options that should be initially selected.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="selector">A function that selects the initial options from the available options.</param>
    /// <returns>The same instance so calls can be chained.</returns>
    public static InputElementBuilder<CheckboxGroup> InitialOptions(this InputElementBuilder<CheckboxGroup> builder,
        Func<IList<Option>, IList<Option>> selector) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Modify(x => x.InitialOptions = selector(x.Options));

    /// <summary>
    /// Pre-selects options in the checkbox group by their values.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="initialOptions">The values of the options to select initially.</param>
    /// <returns>The same instance so calls can be chained.</returns>
    public static InputElementBuilder<CheckboxGroup> InitialOptions(this InputElementBuilder<CheckboxGroup> builder,
        params string[] initialOptions) =>
        builder.InitialOptions(options => options.Where(o => initialOptions.Contains(o.Value)).ToList());
}