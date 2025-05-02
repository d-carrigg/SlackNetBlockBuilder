using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Provides extension methods for configuring <see cref="RadioButtonGroup"/> elements.
/// </summary>
[PublicAPI]
public static class RadioButtonGroupExtensions
{
    /// <summary>
    /// Adds an option to the radio button group.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="value">The string value that will be passed to your app when this option is chosen. Maximum length 75 characters.</param>
    /// <param name="text">A plain text object that defines the text shown next to the radio button. Maximum length 75 characters.</param>
    /// <param name="description">An optional plain text object shown below the <paramref name="text"/> field. Maximum length 75 characters.</param>
    /// <summary>
        /// Adds a new option to the radio button group with the specified value, label, and optional description.
        /// </summary>
        /// <param name="value">The value associated with the option (maximum 75 characters).</param>
        /// <param name="text">The plain text label displayed next to the radio button (maximum 75 characters).</param>
        /// <param name="description">Optional plain text shown below the label (maximum 75 characters).</param>
        /// <returns>The same builder instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is null.</exception>
    public static InputElementBuilder<RadioButtonGroup> AddOption(this InputElementBuilder<RadioButtonGroup> builder,
        string value,
        string text, PlainText? description = null) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x =>
        x.Options.Add(new Option { Text = text, Value = value, Description = description }));

    /// <summary>
    /// Sets the option that should be initially selected in the radio button group.
    /// The matching option is found by comparing the provided <paramref name="value"/> with the <see cref="Option.Value"/> of the options already added.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="value">The value string of the option to select initially.</param>
    /// <summary>
        /// Sets the initially selected option in the radio button group by matching the provided value.
        /// </summary>
        /// <param name="builder">The builder instance to configure.</param>
        /// <param name="value">The value of the option to select initially.</param>
        /// <returns>The same builder instance for method chaining.</returns>
    public static InputElementBuilder<RadioButtonGroup> InitialOption(
        this InputElementBuilder<RadioButtonGroup> builder,
        string value) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialOption = x.Options.FirstOrDefault(o => o.Value == value));

    /// <summary>
    /// Indicates whether the element will be set to autofocus within the view object.
    /// Only one element can be set to true. Defaults to false.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="focus">True to focus on load.</param>
    /// <summary>
        /// Sets whether the radio button group should automatically receive focus when the view loads.
        /// </summary>
        /// <param name="focus">If true, the radio button group will be focused on load; otherwise, it will not. Defaults to true.</param>
        /// <returns>The same builder instance for method chaining.</returns>
    public static InputElementBuilder<RadioButtonGroup> FocusOnLoad(
        this InputElementBuilder<RadioButtonGroup> builder,
        bool focus = true) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.FocusOnLoad = focus);
}