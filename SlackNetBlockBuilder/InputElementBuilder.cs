namespace SlackNet.Blocks;

/// <summary>
/// Base builder for configuring input elements that implement <see cref="IActionElement"/> and <see cref="IInputBlockElement"/>.
/// Provides access to the underlying element and common configuration methods like setting the action ID.
/// </summary>
/// <typeparam name="TElement">The type of the input element being configured.</typeparam>
public class InputElementBuilder<TElement>
    where TElement : class, IActionElement, IInputBlockElement
{
    /// <summary>
    /// Gets the input element instance being configured.
    /// </summary>
    public TElement Element { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InputElementBuilder{TElement}"/> class.
    /// </summary>
    /// <summary>
    /// Initializes a new instance of the <see cref="InputElementBuilder{TElement}"/> class for configuring the specified input element.
    /// </summary>
    /// <param name="element">The input element to be configured. Must not be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="element"/> is null.</exception>
    public InputElementBuilder(TElement element)
    {
        ArgumentNullException.ThrowIfNull(element);
        Element = element;
    }
    
    /// <summary>
    /// Allows applying custom modifications to the underlying <see cref="Element"/>.
    /// </summary>
    /// <param name="modifier">An action that modifies the <see cref="Element"/>.</param>
    /// <summary>
    /// Applies a custom modification to the input element being built.
    /// </summary>
    /// <param name="modifier">An action that modifies the underlying input element.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public InputElementBuilder<TElement> Set(Action<TElement> modifier)
    {
        ArgumentNullException.ThrowIfNull(modifier);
        modifier(Element);
        return this;
    }

    /// <summary>
    /// Sets an identifier for this action. Should be unique within the block.
    /// Maximum length 255 characters.
    /// </summary>
    /// <param name="actionId">The action ID.</param>
    /// <summary>
    /// Sets the action ID for the input element.
    /// </summary>
    /// <param name="actionId">A unique identifier for this action within the block, up to 255 characters.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public InputElementBuilder<TElement> ActionId(string actionId)
    {
        Element.ActionId = actionId;
        return this;
    }
}