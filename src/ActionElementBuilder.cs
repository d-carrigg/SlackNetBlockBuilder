namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="ActionElement"/>
/// </summary>
/// <typeparam name="TElement">The type of action element to build</typeparam>
public sealed class ActionElementBuilder<TElement>
    where TElement : ActionElement
{
    /// <summary>
    /// The action element instance being built.
    /// </summary>
    public TElement Element { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActionElementBuilder{TElement}"/> class.
    /// </summary>
    /// <param name="element">The action element to wrap.</param>
    public ActionElementBuilder(TElement element)
    {
        Element = element;
    }
    

    /// <summary>
    /// Adds a confirmation dialog to the element that will be displayed when the element is activated
    /// </summary>
    /// <param name="createDialog">An action which configures the dialog</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionElementBuilder<TElement> ConfirmationDialog(Action<ConfirmationDialog> createDialog)
    {
        ArgumentNullException.ThrowIfNull(createDialog);
        Element.Confirm = new ConfirmationDialog();
        createDialog(Element.Confirm);
        return this;
    }


    /// <summary>
    /// Allows applying modifications to the underlying element instance.
    /// This is primarily intended for use by extension methods.
    /// </summary>
    /// <param name="modifier">An action that modifies the element.</param>
    /// <returns>The same instance so calls can be chained.</returns>
    public ActionElementBuilder<TElement> Set(Action<TElement> modifier)
    {
        ArgumentNullException.ThrowIfNull(modifier);
        modifier(Element);
        return this;
    }
    
    /// <summary>
    /// Sets the Action ID for this element.
    /// </summary>
    /// <param name="actionId">An identifier for this action. You can use this when you receive an interaction payload to identify the source of the action. Should be unique among all other action_ids used elsewhere by your app. Maximum length is 255 characters.</param>
    /// <returns>The same instance so calls can be chained.</returns>
    public ActionElementBuilder<TElement> ActionId(string actionId)
    {
        Element.ActionId = actionId;
        return this;
    }
}