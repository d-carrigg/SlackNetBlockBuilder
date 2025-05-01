namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="ActionElement"/>
/// </summary>
/// <typeparam name="TElement">The type of action element to build</typeparam>
public sealed class ActionElementBuilder<TElement>
    // : ActionElementBuilderBase<TElement>
    where TElement : ActionElement
{
    public TElement Element { get; }

    public ActionElementBuilder(TElement element)
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
    public ActionElementBuilder<TElement> ConfirmationDialog(Action<ConfirmationDialog> createDialog)
    {
        Element.Confirm = new ConfirmationDialog();
        createDialog(Element.Confirm);
        return this;
    }


    public ActionElementBuilder<TElement> Set(Action<TElement> modifier)
    {
        modifier(Element);
        return this;
    }
    
    public ActionElementBuilder<TElement> ActionId(string actionId)
    {
        Element.ActionId = actionId;
        return this;
    }
}