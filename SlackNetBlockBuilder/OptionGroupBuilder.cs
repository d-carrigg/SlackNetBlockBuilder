namespace SlackNet.Blocks;

public sealed class OptionGroupBuilder
{
    public OptionGroup Element { get; }

    public OptionGroupBuilder(OptionGroup element)
    {
        Element = element;
    }


    public OptionGroupBuilder AddOption(string value, string text, PlainText description = null)
    {
        Element.Options.Add(new Option { Text = text, Value = value, Description = description });
        return this;
    }

    public OptionGroupBuilder AddOption(Option option)
    {
        Element.Options.Add(option);
        return this;
    }
}