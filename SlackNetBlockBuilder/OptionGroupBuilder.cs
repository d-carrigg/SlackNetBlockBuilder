namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building option groups
/// </summary>
public sealed class OptionGroupBuilder
{
    /// <summary>
    /// Gets the option group being built
    /// </summary>
    public OptionGroup Element { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionGroupBuilder"/> class
    /// </summary>
    /// <param name="element">The option group to build</param>
    public OptionGroupBuilder(OptionGroup element)
    {
        Element = element;
    }

    /// <summary>
    /// Adds an option to the option group
    /// </summary>
    /// <param name="value">The value to be sent to your app when the option is selected</param>
    /// <param name="text">The text to display for the option</param>
    /// <param name="description">Optional description for the option</param>
    /// <returns>The same instance so calls can be chained</returns>
    public OptionGroupBuilder AddOption(string value, string text, PlainText description = null)
    {
        Element.Options.Add(new Option { Text = text, Value = value, Description = description });
        return this;
    }

    /// <summary>
    /// Adds an existing option to the option group
    /// </summary>
    /// <param name="option">The option to add</param>
    /// <returns>The same instance so calls can be chained</returns>
    public OptionGroupBuilder AddOption(Option option)
    {
        Element.Options.Add(option);
        return this;
    }
}