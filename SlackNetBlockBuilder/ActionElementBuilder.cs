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
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionElementBuilder{TElement}"/> class for building and configuring the specified action element.
    /// </summary>
    /// <param name="element">The action element to be wrapped and configured by the builder.</param>
    public ActionElementBuilder(TElement element)
    {
        Element = element;
    }
    

    /// <summary>
    /// Adds a confirmation dialog to the element that will be displayed when the element is activated
    /// </summary>
    /// <param name="createDialog">An action which configures the dialog</param>
    /// <summary>
    /// Adds and configures a confirmation dialog for the action element.
    /// </summary>
    /// <param name="createDialog">A delegate to configure the confirmation dialog.</param>
    /// <returns>The current builder instance for method chaining.</returns>
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
    /// <summary>
    /// Applies a custom modification to the underlying action element.
    /// </summary>
    /// <param name="modifier">A delegate that modifies the wrapped action element.</param>
    /// <returns>The current builder instance for method chaining.</returns>
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
    /// <summary>
    /// Sets the unique action identifier for the underlying element.
    /// </summary>
    /// <param name="actionId">A string identifier for the action (maximum 255 characters).</param>
    /// <returns>The current builder instance for method chaining.</returns>
    public ActionElementBuilder<TElement> ActionId(string actionId)
    {
        Element.ActionId = actionId;
        return this;
    }
}