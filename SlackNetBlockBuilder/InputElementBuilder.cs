namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building input elements
/// </summary>
/// <typeparam name="TElement">The type of input element to build</typeparam>
public class InputElementBuilder<TElement>
    // : ActionElementBuilderBase<TElement>
    where TElement : class, IActionElement, IInputBlockElement
{
    /// <summary>
    /// Gets the input element being built
    /// </summary>
    public TElement Element { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InputElementBuilder{TElement}"/> class
    /// </summary>
    /// <param name="element">The input element to build</param>
    public InputElementBuilder(TElement element)
    {
        Element = element;
    }

    

    /// <summary>
    /// Sets a property on the element using the provided modifier action
    /// </summary>
    /// <param name="modifier">The action to modify the element</param>
    /// <returns>The same instance so calls can be chained</returns>
    public InputElementBuilder<TElement> Set(Action<TElement> modifier)
    {
        modifier(Element);
        return this;
    }
    
    /// <summary>
    /// Sets the action ID for the element
    /// </summary>
    /// <param name="actionId">The action ID to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public InputElementBuilder<TElement> ActionId(string actionId)
    {
        Element.ActionId = actionId;
        return this;
    }
}