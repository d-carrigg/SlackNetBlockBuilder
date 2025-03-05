namespace SlackNet.Blocks;

// public interface IActionElementBuilder<TElement> where TElement : IActionElement
// {
//     TElement Element { get; }
//     //IActionElementBuilder<TElement> Set(Action<TElement> modifier);
// }
//
// public sealed class InputBlockBuilder<TElement> : ActionElementBuilderBase<TElement>
//     where TElement : Block, IInputBlockElement
// {
//     public InputBlockBuilder(TElement element) : base(element)
//     {
//     }
// }
//
// public static class ActionElementBuilderExtensions
// {
//     /// <summary>
//     /// Set a property on the element
//     /// </summary>
//     /// <param name="modifier">The modifier</param>
//     /// <returns>The same instance to calls can be chained</returns>
//     public static TBuilder Set<TBuilder, TElement>(this TBuilder builder, Action<TElement> modifier) 
//         where TBuilder : IActionElementBuilder<TElement> where TElement : IActionElement
//     {
//         modifier(builder.Element);
//         return builder;
//     }
// }
//
// public abstract class ActionElementBuilderBase<TElement> : IActionElementBuilder<TElement> where TElement : IActionElement
// {
//     
//     public TElement Element { get; }
//  
//
//     protected ActionElementBuilderBase(TElement element)
//     {
//         Element = element;
//     }
//     
//
// }

/// <summary>
/// Provides a fluent interface for building action elements
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