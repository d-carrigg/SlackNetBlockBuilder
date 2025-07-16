namespace SlackNet.Blocks;
/// <summary>
/// Extension methods for <see cref="InputElementBuilder{TElement}"/> to provide additional configuration options.
/// </summary>
public static class InputBuilderExtensions
{
    /// <summary>
    /// Sets the placeholder text shown in the input element.
    /// Maximum length 150 characters.
    /// </summary>
    /// <typeparam name="TElement">The type of the select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="placeholder">The placeholder text.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> Placeholder<TElement>(
        this InputElementBuilder<TElement> builder,
        string placeholder) where TElement : PlainTextInput
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
                builder.Set(x => x.Placeholder = placeholder);
    
}