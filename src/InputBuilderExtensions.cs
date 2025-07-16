using JetBrains.Annotations;

namespace SlackNet.Blocks;
/// <summary>
/// Extension methods for <see cref="InputElementBuilder{TElement}"/> to provide additional configuration options.
/// </summary>
[PublicAPI]
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
    
    
    /// <summary>
    /// Sets whether the input element should be a multiline text area.
    /// </summary>
    /// <typeparam name="TElement">The type of the select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="multiline">If true, the input element will be a multiline text area; otherwise, it will be a single-line input.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> Multiline<TElement>(
        this InputElementBuilder<TElement> builder,
        bool multiline = true) where TElement : PlainTextInput
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
                builder.Set(x => x.Multiline = multiline);
    
 
    /// <summary>
    ///  Sets the minimum length of input that the user must provide.
    /// </summary>
    /// <typeparam name="TElement">The type of the select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="minLength">The minimum length of input required.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> MinLength<TElement>(
        this InputElementBuilder<TElement> builder,
        int minLength) where TElement : PlainTextInput
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
                builder.Set(x => x.MinLength = minLength);
    /// <summary>
    ///  Sets the maximum length of input that the user must provide.
    /// </summary>
    /// <typeparam name="TElement">The type of the select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="maxLength">The maximum length of input required.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> MaxLength<TElement>(
        this InputElementBuilder<TElement> builder,
        int maxLength) where TElement : PlainTextInput
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
                builder.Set(x => x.MaxLength = maxLength);
    
}