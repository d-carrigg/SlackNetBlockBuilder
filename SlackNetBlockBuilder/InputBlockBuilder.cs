namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="InputBlock"/>
/// </summary>
/// <typeparam name="TElement">The type of input element in the block</typeparam>
public sealed class InputBlockBuilder<TElement> : InputElementBuilder<TElement>
    where TElement : IActionElement, IInputBlockElement
{
    /// <summary>
    /// Gets the input block being built
    /// </summary>
    public InputBlock ParentBlock { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InputBlockBuilder{TElement}"/> class
    /// </summary>
    /// <param name="element">The input element to include in the block</param>
    /// <param name="label">The label for the input</param>
    public InputBlockBuilder(TElement element, string label) : base(element)
    {
        ParentBlock = new InputBlock()
            {
                Label = label,
                Element = element
            };
    }

    /// <summary>
    /// Sets the block ID for the input block
    /// </summary>
    /// <param name="blockId">The block ID to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public InputElementBuilder<TElement> BlockId(string blockId)
    {
        ParentBlock.BlockId = blockId;
        return this;
    }

    /// <summary>
    /// Sets whether the input should dispatch an action when the user interacts with it
    /// </summary>
    /// <param name="dispatch">Whether to dispatch an action</param>
    /// <returns>The same instance so calls can be chained</returns>
    public InputElementBuilder<TElement> DispatchAction(bool dispatch = true)
    {
        ParentBlock.DispatchAction = dispatch;
        return this;
    }

    /// <summary>
    /// Sets a hint that appears below the input to provide additional context
    /// </summary>
    /// <param name="hint">The hint text</param>
    /// <returns>The same instance so calls can be chained</returns>
    public InputElementBuilder<TElement> Hint(string hint)
    {
        ParentBlock.Hint = hint;
        return this;
    }

    /// <summary>
    /// Sets whether the input is optional
    /// </summary>
    /// <param name="optional">Whether the input is optional</param>
    /// <returns>The same instance so calls can be chained</returns>
    public InputElementBuilder<TElement> Optional(bool optional = true)
    {
        ParentBlock.Optional = optional;
        return this;
    }
}