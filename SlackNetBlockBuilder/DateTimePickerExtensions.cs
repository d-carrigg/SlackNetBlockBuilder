using JetBrains.Annotations;

namespace SlackNet.Blocks;
 
[PublicAPI]
public static class DateTimePickerExtensions
{
    // Date Picker
    public static InputElementBuilder<DatePicker> InitialDate(this InputElementBuilder<DatePicker> builder,
        DateTime? initialDate) => builder.Set(x => x.InitialDate = initialDate);


    public static InputElementBuilder<DatePicker> Placeholder(this InputElementBuilder<DatePicker> builder,
        string placeholder) => builder.Set(x => x.Placeholder = placeholder);

    public static InputElementBuilder<DatePicker> FocusOnLoad(this InputElementBuilder<DatePicker> builder,
        bool focus = true)
        => builder.Set(x => x.FocusOnLoad = focus);

    // Time Picker

    public static InputElementBuilder<TimePicker> InitialTime(this InputElementBuilder<TimePicker> builder,
        TimeSpan? initialTime) => builder.Set(x => x.InitialTime = initialTime);

    public static InputElementBuilder<TimePicker> Placeholder(this InputElementBuilder<TimePicker> builder,
        string placeholder) => builder.Set(x => x.Placeholder = placeholder);

    public static InputElementBuilder<TimePicker> FocusOnLoad(this InputElementBuilder<TimePicker> builder,
        bool focus = true)
        => builder.Set(x => x.FocusOnLoad = focus);

    // Date Time Picker

    public static InputElementBuilder<DateTimePicker> InitialDateTime(
        this InputElementBuilder<DateTimePicker> builder,
        DateTime? initialDateTime) => builder.Set(x => x.InitialDateTime = initialDateTime);

    public static InputElementBuilder<DateTimePicker> FocusOnLoad(this InputElementBuilder<DateTimePicker> builder,
        bool focus = true)
        => builder.Set(x => x.FocusOnLoad = focus);
}