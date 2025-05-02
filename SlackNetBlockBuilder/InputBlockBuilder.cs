namespace SlackNet.Blocks;

public sealed class InputBlockBuilder<TElement> : InputElementBuilder<TElement>
    where TElement : IActionElement, IInputBlockElement
{
    public InputBlock ParentBlock { get; }

    public InputBlockBuilder(TElement element, string label) : base(element)
    {
        ParentBlock = new InputBlock()
            {
                Label = label,
                Element = element
            };
    }

    public InputElementBuilder<TElement> BlockId(string blockId)
    {
        ParentBlock.BlockId = blockId;
        return this;
    }

    public InputElementBuilder<TElement> DispatchAction(bool dispatch = true)
    {
        ParentBlock.DispatchAction = dispatch;
        return this;
    }

    public InputElementBuilder<TElement> Hint(string hint)
    {
        ParentBlock.Hint = hint;
        return this;
    }

    public InputElementBuilder<TElement> Optional(bool optional = true)
    {
        ParentBlock.Optional = optional;
        return this;
    }
}