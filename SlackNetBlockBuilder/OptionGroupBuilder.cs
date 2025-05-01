namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building an <see cref="OptionGroup"/>.
/// Option groups are used to categorize options within select menus.
/// </summary>
public sealed class OptionGroupBuilder
{
    /// <summary>
    /// Gets the <see cref="OptionGroup"/> instance being configured.
    /// </summary>
    public OptionGroup Element { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionGroupBuilder"/> class.
    /// </summary>
    /// <param name="element">The <see cref="OptionGroup"/> instance to configure.</param>
    public OptionGroupBuilder(OptionGroup element)
    {
        Element = element;
    }


    /// <summary>
    /// Adds an <see cref="Option"/> to the group.
    /// </summary>
    /// <param name="value">The string value that will be passed to your app when this option is chosen. Maximum length 75 characters.</param>
    /// <param name="text">A plain text object that defines the text shown in the option on the menu. Maximum length 75 characters.</param>
    /// <param name="description">An optional plain text object shown below the <paramref name="text"/> field. Maximum length 75 characters.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public OptionGroupBuilder AddOption(string value, string text, PlainText? description = null)
    {
        Element.Options ??= new List<Option>();
        Element.Options.Add(new Option { Text = text, Value = value, Description = description });
        return this;
    }

    /// <summary>
    /// Adds a pre-configured <see cref="Option"/> object to the group.
    /// </summary>
    /// <param name="option">The <see cref="Option"/> to add.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public OptionGroupBuilder AddOption(Option option)
    {
        Element.Options.Add(option);
        return this;
    }
}