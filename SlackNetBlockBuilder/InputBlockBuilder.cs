namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building an <see cref="InputBlock"/> that contains a specific input element of type <typeparamref name="TElement"/>.
/// This builder allows configuration of both the <see cref="InputBlock"/> properties and the contained input element.
/// </summary>
/// <typeparam name="TElement">The type of the input element contained within the block.</typeparam>
public sealed class InputBlockBuilder<TElement> : InputElementBuilder<TElement>
    where TElement : IActionElement, IInputBlockElement
{
    /// <summary>
    /// Gets the <see cref="InputBlock"/> instance being configured by this builder.
    /// </summary>
    public InputBlock ParentBlock { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InputBlockBuilder{TElement}"/> class.
    /// </summary>
    /// <param name="element">The input element to be placed within the block.</param>
    /// <param name="label">A label shown above the input element. Maximum length 2000 characters.</param>
    public InputBlockBuilder(TElement element, string label) : base(element)
    {
        ParentBlock = new InputBlock()
            {
                Label = label,
                Element = element
            };
    }

    /// <summary>
    /// Sets a unique identifier for this input block.
    /// Maximum length 255 characters.
    /// </summary>
    /// <param name="blockId">The block ID.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public InputElementBuilder<TElement> BlockId(string blockId)
    {
        ParentBlock.BlockId = blockId;
        return this;
    }

    /// <summary>
    /// Determines whether block actions are dispatched with an interaction payload.
    /// Should be set to true if the element may trigger an interaction during value selection (e.g. select menus).
    /// Defaults to false.
    /// </summary>
    /// <param name="dispatch">True to dispatch actions.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public InputElementBuilder<TElement> DispatchAction(bool dispatch = true)
    {
        ParentBlock.DispatchAction = dispatch;
        return this;
    }

    /// <summary>
    /// Sets hint text that appears below the input element.
    /// Maximum length 2000 characters.
    /// </summary>
    /// <param name="hint">The hint text.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public InputElementBuilder<TElement> Hint(string hint)
    {
        ParentBlock.Hint = hint;
        return this;
    }

    /// <summary>
    /// Sets whether the input element is optional. Defaults to false.
    /// </summary>
    /// <param name="optional">True if the input is optional.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public InputElementBuilder<TElement> Optional(bool optional = true)
    {
        ParentBlock.Optional = optional;
        return this;
    }
}