namespace SlackNet.Blocks;

public class  InputElementBuilder<TElement>
    // : ActionElementBuilderBase<TElement>
    where TElement : IActionElement, IInputBlockElement
{
    public TElement Element { get; }


    public InputElementBuilder(TElement element)
    {
        Element = element;
    }


    // public ActionElementBuilder(TElement element) : base(element)
    // {
    // }

    /// <summary>
    /// Adds a confirmation dialog to the element that will be displayed when the element is activated
    /// </summary>
    /// <param name="createDialog">An action which configures the dialog</param>
    /// <returns>The same instance so calls can be chained</returns>
    // public InputElementBuilder<TElement> ConfirmationDialog(Action<ConfirmationDialog> createDialog)
    // {
    //     Element.Confirm = new ConfirmationDialog();
    //     createDialog(Element.Confirm);
    //     return this;
    // }


    public InputElementBuilder<TElement> Set(Action<TElement> modifier)
    {
        modifier(Element);
        return this;
    }
    
    public InputElementBuilder<TElement> ActionId(string actionId)
    {
        Element.ActionId = actionId;
        return this;
    }
}