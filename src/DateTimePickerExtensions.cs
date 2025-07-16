using JetBrains.Annotations;

namespace SlackNet.Blocks;
 
/// <summary>
/// Provides extension methods for configuring <see cref="DatePicker"/>, <see cref="TimePicker"/>, and <see cref="DateTimePicker"/> elements.
/// </summary>
[PublicAPI]
public static class DateTimePickerExtensions
{
    // === Date Picker ===

    /// <summary>
    /// Sets the initial date that is selected when the date picker element loads.
    /// Uses the format "YYYY-MM-DD".
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="initialDate">The initial date to select.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<DatePicker> InitialDate(this InputElementBuilder<DatePicker> builder,
        DateTime? initialDate) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Modify(x => x.InitialDate = initialDate);


    /// <summary>
    /// Sets the placeholder text shown on the date picker element. Maximum length 150 characters.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="placeholder">The placeholder text.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<DatePicker> Placeholder(this InputElementBuilder<DatePicker> builder,
        string placeholder) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        string.IsNullOrEmpty(placeholder) ? throw new ArgumentNullException(nameof(placeholder)) :
        builder.Modify(x => x.Placeholder = placeholder);

    /// <summary>
    /// Indicates whether the element will be set to autofocus within the view object.
    /// Only one element can be set to true. Defaults to false.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="focus">True to focus on load.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<DatePicker> FocusOnLoad(this InputElementBuilder<DatePicker> builder,
        bool focus = true)
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Modify(x => x.FocusOnLoad = focus);

    // === Time Picker ===

    /// <summary>
    /// Sets the initial time that is selected when the time picker element loads.
    /// Uses the 24-hour format "HH:mm".
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="initialTime">The initial time to select.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TimePicker> InitialTime(this InputElementBuilder<TimePicker> builder,
        TimeSpan? initialTime) =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Modify(x => x.InitialTime = initialTime);

    /// <summary>
    /// Sets the placeholder text shown on the time picker element. Maximum length 150 characters.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="placeholder">The placeholder text.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TimePicker> Placeholder(this InputElementBuilder<TimePicker> builder,
        string placeholder) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Modify(x => x.Placeholder = placeholder);

    /// <summary>
    /// Indicates whether the element will be set to autofocus within the view object.
    /// Only one element can be set to true. Defaults to false.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="focus">True to focus on load.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TimePicker> FocusOnLoad(this InputElementBuilder<TimePicker> builder,
        bool focus = true)
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Modify(x => x.FocusOnLoad = focus);

    // === Date Time Picker ===

    /// <summary>
    /// Sets the initial date and time that is selected when the datetime picker element loads.
    /// This must be a Unix timestamp (seconds since the epoch).
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="initialDateTime">The initial date and time to select.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<DateTimePicker> InitialDateTime(
        this InputElementBuilder<DateTimePicker> builder,
        DateTime? initialDateTime) =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Modify(x => x.InitialDateTime = initialDateTime);

    /// <summary>
    /// Indicates whether the element will be set to autofocus within the view object.
    /// Only one element can be set to true. Defaults to false.
    /// </summary>
    /// <param name="builder">The builder instance.</param>
    /// <param name="focus">True to focus on load.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<DateTimePicker> FocusOnLoad(this InputElementBuilder<DateTimePicker> builder,
        bool focus = true)
        =>
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Modify(x => x.FocusOnLoad = focus);
}