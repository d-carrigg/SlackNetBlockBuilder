using JetBrains.Annotations;
using SlackNet.Blocks;

namespace SlackNetBlockBuilder;

/// <summary>
/// Provides a fluent interface for building <see cref="ActionsBlock"/>
/// </summary>
[PublicAPI]
public sealed class ActionsBlockBuilder
{

    public const int MaxElements = 25;
    
    public const int MaxBlockIdLength = 255;
    
    
    private ActionsBlockBuilder()
    {
        Element = new();
    }
    public static ActionsBlockBuilder Create() => new();
    public ActionsBlock Build()
    {
        if (Element.Elements.Count > MaxElements)
        {
            throw new InvalidOperationException($"An actions block can only contain up to {MaxElements} elements");
        }
        
        if(Element.BlockId?.Length > MaxBlockIdLength)
        {
            throw new InvalidOperationException($"The block id can only be up to {MaxBlockIdLength} characters long");
        }
        
        return Element;
    }

    private readonly ActionsBlock Element;


    public ActionsBlockBuilder WithBlockId(string blockId)
    {
        Element.BlockId = blockId;
        return this;
    }

    public ActionsBlockBuilder AddElement<TElement>(string actionId, TElement element,
        Action<ActionElementBuilder<TElement>> createElement) where TElement : ActionElement
    {
        element.ActionId = actionId;
        createElement(new ActionElementBuilder<TElement>(element));
        Element.Elements.Add(element);

        return this;
    }

    public ActionsBlockBuilder AddElement<TElement>(string actionId,
        Action<ActionElementBuilder<TElement>> createElement) where TElement : ActionElement, new()
    {
        var element = new TElement();
        element.ActionId = actionId;
        createElement(new ActionElementBuilder<TElement>(element));
        Element.Elements.Add(element);

        return this;
    }
    
    public ActionsBlockBuilder AddElement<TElement>(string actionId,
        Action<InputElementBuilder<TElement>> createElement) where TElement : ActionElement, IInputBlockElement, new()
    {
        var element = new TElement();
        element.ActionId = actionId;
        createElement(new InputElementBuilder<TElement>(element));
        Element.Elements.Add(element);

        return this;
    }
    
    public ActionsBlockBuilder AddButton(string actionId, Action<ActionElementBuilder<Button>> createButton)
        => AddElement(actionId, createButton);

    public ActionsBlockBuilder AddCheckboxGroup(string actionId,
        Action<InputElementBuilder<CheckboxGroup>> createCheckbox)
        => AddElement(actionId, createCheckbox);

    public ActionsBlockBuilder AddDatePicker(string actionId, Action<InputElementBuilder<DatePicker>> createDatePicker)
        => AddElement(actionId, createDatePicker);

    public ActionsBlockBuilder AddTimePicker(string actionId, Action<InputElementBuilder<TimePicker>> createTimePicker)
        => AddElement(actionId, createTimePicker);

    public ActionsBlockBuilder AddDateTimePicker(string actionId,
        Action<InputElementBuilder<DateTimePicker>> createDateTimePicker)
        => AddElement(actionId, createDateTimePicker);


    public ActionsBlockBuilder AddOverflowMenu(string actionId,
        Action<ActionElementBuilder<OverflowMenu>> createOverflowMenu) => AddElement(actionId, createOverflowMenu);

    public ActionsBlockBuilder AddRadioButtonGroup(string actionId,
        Action<InputElementBuilder<RadioButtonGroup>> createRadioButtonGroup)
        => AddElement(actionId, createRadioButtonGroup);

    public ActionsBlockBuilder AddStaticSelectMenu(string actionId,
        Action<InputElementBuilder<StaticSelectMenu>> createStaticSelect)
        => AddElement(actionId, createStaticSelect);

    public ActionsBlockBuilder AddExternalSelectMenu(string actionId,
        Action<InputElementBuilder<ExternalSelectMenu>> createExternalSelect)
        => AddElement(actionId, createExternalSelect);

    public ActionsBlockBuilder AddUserSelectMenu(string actionId,
        Action<InputElementBuilder<UserSelectMenu>> createUserSelect)
        => AddElement(actionId, createUserSelect);

    public ActionsBlockBuilder AddConversationSelectMenu(string actionId,
        Action<InputElementBuilder<ConversationSelectMenu>> createConversationsSelect)
        => AddElement(actionId, createConversationsSelect);

    public ActionsBlockBuilder AddChannelSelectMenu(string actionId,
        Action<InputElementBuilder<ChannelSelectMenu>> createChannelSelect)
        => AddElement(actionId, createChannelSelect);

    public ActionsBlockBuilder AddMultiStaticSelectMenu(string actionId,
        Action<InputElementBuilder<StaticMultiSelectMenu>> createMultiStaticSelect)
        => AddElement(actionId, createMultiStaticSelect);


    public ActionsBlockBuilder AddMultiExternalSelectMenu(string actionId,
        Action<InputElementBuilder<ExternalMultiSelectMenu>> createMultiExternalSelect)
        => AddElement(actionId, createMultiExternalSelect);

    public ActionsBlockBuilder AddMultiUserSelectMenu(string actionId,
        Action<InputElementBuilder<UserMultiSelectMenu>> createMultiUserSelect)
        => AddElement(actionId, createMultiUserSelect);

    public ActionsBlockBuilder AddMultiConversationSelectMenu(string actionId,
        Action<InputElementBuilder<ConversationMultiSelectMenu>> createMultiConversationsSelect)
        => AddElement(actionId, createMultiConversationsSelect);

    public ActionsBlockBuilder AddMultiChannelSelectMenu(string actionId,
        Action<InputElementBuilder<ChannelMultiSelectMenu>> createMultiChannelSelect)
        => AddElement(actionId, createMultiChannelSelect);


    
}