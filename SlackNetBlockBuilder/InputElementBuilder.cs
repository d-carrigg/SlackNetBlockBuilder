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
    /// <param name="element">The input element instance to configure.</param>
    public InputElementBuilder(TElement element)
    {
        Element = element;
    }
    
    /// <summary>
    /// Allows applying custom modifications to the underlying <see cref="Element"/>.
    /// </summary>
    /// <param name="modifier">An action that modifies the <see cref="Element"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public InputElementBuilder<TElement> Set(Action<TElement> modifier)
    {
        modifier(Element);
        return this;
    }

    /// <summary>
    /// Sets an identifier for this action. Should be unique within the block.
    /// Maximum length 255 characters.
    /// </summary>
    /// <param name="actionId">The action ID.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public InputElementBuilder<TElement> ActionId(string actionId)
    {
        Element.ActionId = actionId;
        return this;
    }
}