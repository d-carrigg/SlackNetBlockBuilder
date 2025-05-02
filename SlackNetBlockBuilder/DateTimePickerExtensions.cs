using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Extension methods for working with date and time picker elements
/// </summary>
[PublicAPI]
public static class DateTimePickerExtensions
{
    // Date Picker
    
    /// <summary>
    /// Sets the initial date for a date picker
    /// </summary>
    /// <param name="builder">The date picker builder</param>
    /// <param name="initialDate">The initial date to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<DatePicker> InitialDate(this InputElementBuilder<DatePicker> builder,
        DateTime? initialDate) => builder.Set(x => x.InitialDate = initialDate);

    /// <summary>
    /// Sets the placeholder text for a date picker
    /// </summary>
    /// <param name="builder">The date picker builder</param>
    /// <param name="placeholder">The placeholder text to display</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<DatePicker> Placeholder(this InputElementBuilder<DatePicker> builder,
        string placeholder) => builder.Set(x => x.Placeholder = placeholder);

    /// <summary>
    /// Sets whether the date picker should be focused when the view is opened
    /// </summary>
    /// <param name="builder">The date picker builder</param>
    /// <param name="focus">Whether to focus the element</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<DatePicker> FocusOnLoad(this InputElementBuilder<DatePicker> builder,
        bool focus = true)
        => builder.Set(x => x.FocusOnLoad = focus);

    // Time Picker

    /// <summary>
    /// Sets the initial time for a time picker
    /// </summary>
    /// <param name="builder">The time picker builder</param>
    /// <param name="initialTime">The initial time to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TimePicker> InitialTime(this InputElementBuilder<TimePicker> builder,
        TimeSpan? initialTime) => builder.Set(x => x.InitialTime = initialTime);

    /// <summary>
    /// Sets the placeholder text for a time picker
    /// </summary>
    /// <param name="builder">The time picker builder</param>
    /// <param name="placeholder">The placeholder text to display</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TimePicker> Placeholder(this InputElementBuilder<TimePicker> builder,
        string placeholder) => builder.Set(x => x.Placeholder = placeholder);

    /// <summary>
    /// Sets whether the time picker should be focused when the view is opened
    /// </summary>
    /// <param name="builder">The time picker builder</param>
    /// <param name="focus">Whether to focus the element</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TimePicker> FocusOnLoad(this InputElementBuilder<TimePicker> builder,
        bool focus = true)
        => builder.Set(x => x.FocusOnLoad = focus);

    // Date Time Picker

    /// <summary>
    /// Sets the initial date and time for a date-time picker
    /// </summary>
    /// <param name="builder">The date-time picker builder</param>
    /// <param name="initialDateTime">The initial date and time to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<DateTimePicker> InitialDateTime(
        this InputElementBuilder<DateTimePicker> builder,
        DateTime? initialDateTime) => builder.Set(x => x.InitialDateTime = initialDateTime);

    /// <summary>
    /// Sets whether the date-time picker should be focused when the view is opened
    /// </summary>
    /// <param name="builder">The date-time picker builder</param>
    /// <param name="focus">Whether to focus the element</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<DateTimePicker> FocusOnLoad(this InputElementBuilder<DateTimePicker> builder,
        bool focus = true)
        => builder.Set(x => x.FocusOnLoad = focus);
}