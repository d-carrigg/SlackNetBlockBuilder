using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="ActionsBlock"/>
/// </summary>
[PublicAPI]
public sealed class ActionsBlockBuilder
{
    /// <summary>
    /// The maximum number of elements allowed in an actions block
    /// </summary>
    public const int MaxElements = 25;
    
    /// <summary>
    /// The maximum length of a block ID
    /// </summary>
    public const int MaxBlockIdLength = 255;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionsBlockBuilder"/> class
    /// </summary>
    private ActionsBlockBuilder()
    {
        Element = new();
    }
    
    /// <summary>
    /// Creates a new instance of the <see cref="ActionsBlockBuilder"/> class
    /// </summary>
    /// <returns>A new ActionsBlockBuilder instance</returns>
    public static ActionsBlockBuilder Create() => new();
    
    /// <summary>
    /// Builds the actions block
    /// </summary>
    /// <returns>The built actions block</returns>
    /// <exception cref="InvalidOperationException">Thrown when the block contains too many elements or the block ID is too long</exception>
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

    /// <summary>
    /// The actions block being built
    /// </summary>
    private readonly ActionsBlock Element;

    /// <summary>
    /// Sets the block ID for the actions block
    /// </summary>
    /// <param name="blockId">The block ID to set</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder WithBlockId(string blockId)
    {
        Element.BlockId = blockId;
        return this;
    }

    /// <summary>
    /// Adds an element to the actions block
    /// </summary>
    /// <typeparam name="TElement">The type of element to add</typeparam>
    /// <param name="actionId">The action ID for the element</param>
    /// <param name="element">The element to add</param>
    /// <param name="createElement">An action to configure the element</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddElement<TElement>(string actionId, TElement element,
        Action<ActionElementBuilder<TElement>> createElement) where TElement : ActionElement
    {
        element.ActionId = actionId;
        createElement(new ActionElementBuilder<TElement>(element));
        Element.Elements.Add(element);

        return this;
    }

    /// <summary>
    /// Adds a new element of the specified type to the actions block
    /// </summary>
    /// <typeparam name="TElement">The type of element to add</typeparam>
    /// <param name="actionId">The action ID for the element</param>
    /// <param name="createElement">An action to configure the element</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddElement<TElement>(string actionId,
        Action<ActionElementBuilder<TElement>> createElement) where TElement : ActionElement, new()
    {
        var element = new TElement();
        element.ActionId = actionId;
        createElement(new ActionElementBuilder<TElement>(element));
        Element.Elements.Add(element);

        return this;
    }
    
    /// <summary>
    /// Adds a new input element of the specified type to the actions block
    /// </summary>
    /// <typeparam name="TElement">The type of element to add</typeparam>
    /// <param name="actionId">The action ID for the element</param>
    /// <param name="createElement">An action to configure the element</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddElement<TElement>(string actionId,
        Action<InputElementBuilder<TElement>> createElement) where TElement : ActionElement, IInputBlockElement, new()
    {
        var element = new TElement();
        element.ActionId = actionId;
        createElement(new InputElementBuilder<TElement>(element));
        Element.Elements.Add(element);

        return this;
    }
    
    /// <summary>
    /// Adds a button to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the button</param>
    /// <param name="createButton">An action to configure the button</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddButton(string actionId, Action<ActionElementBuilder<Button>> createButton)
        => AddElement(actionId, createButton);

    /// <summary>
    /// Adds a checkbox group to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the checkbox group</param>
    /// <param name="createCheckbox">An action to configure the checkbox group</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddCheckboxGroup(string actionId,
        Action<InputElementBuilder<CheckboxGroup>> createCheckbox)
        => AddElement(actionId, createCheckbox);

    /// <summary>
    /// Adds a date picker to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the date picker</param>
    /// <param name="createDatePicker">An action to configure the date picker</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddDatePicker(string actionId, Action<InputElementBuilder<DatePicker>> createDatePicker)
        => AddElement(actionId, createDatePicker);

    /// <summary>
    /// Adds a time picker to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the time picker</param>
    /// <param name="createTimePicker">An action to configure the time picker</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddTimePicker(string actionId, Action<InputElementBuilder<TimePicker>> createTimePicker)
        => AddElement(actionId, createTimePicker);

    /// <summary>
    /// Adds a date-time picker to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the date-time picker</param>
    /// <param name="createDateTimePicker">An action to configure the date-time picker</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddDateTimePicker(string actionId,
        Action<InputElementBuilder<DateTimePicker>> createDateTimePicker)
        => AddElement(actionId, createDateTimePicker);

    /// <summary>
    /// Adds an overflow menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the overflow menu</param>
    /// <param name="createOverflowMenu">An action to configure the overflow menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddOverflowMenu(string actionId,
        Action<ActionElementBuilder<OverflowMenu>> createOverflowMenu) => AddElement(actionId, createOverflowMenu);

    /// <summary>
    /// Adds a radio button group to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the radio button group</param>
    /// <param name="createRadioButtonGroup">An action to configure the radio button group</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddRadioButtonGroup(string actionId,
        Action<InputElementBuilder<RadioButtonGroup>> createRadioButtonGroup)
        => AddElement(actionId, createRadioButtonGroup);

    /// <summary>
    /// Adds a static select menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the select menu</param>
    /// <param name="createStaticSelect">An action to configure the select menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddStaticSelectMenu(string actionId,
        Action<InputElementBuilder<StaticSelectMenu>> createStaticSelect)
        => AddElement(actionId, createStaticSelect);

    /// <summary>
    /// Adds an external select menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the select menu</param>
    /// <param name="createExternalSelect">An action to configure the select menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddExternalSelectMenu(string actionId,
        Action<InputElementBuilder<ExternalSelectMenu>> createExternalSelect)
        => AddElement(actionId, createExternalSelect);

    /// <summary>
    /// Adds a user select menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the select menu</param>
    /// <param name="createUserSelect">An action to configure the select menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddUserSelectMenu(string actionId,
        Action<InputElementBuilder<UserSelectMenu>> createUserSelect)
        => AddElement(actionId, createUserSelect);

    /// <summary>
    /// Adds a conversation select menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the select menu</param>
    /// <param name="createConversationsSelect">An action to configure the select menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddConversationSelectMenu(string actionId,
        Action<InputElementBuilder<ConversationSelectMenu>> createConversationsSelect)
        => AddElement(actionId, createConversationsSelect);

    /// <summary>
    /// Adds a channel select menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the select menu</param>
    /// <param name="createChannelSelect">An action to configure the select menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddChannelSelectMenu(string actionId,
        Action<InputElementBuilder<ChannelSelectMenu>> createChannelSelect)
        => AddElement(actionId, createChannelSelect);

    /// <summary>
    /// Adds a multi-select static select menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the select menu</param>
    /// <param name="createMultiStaticSelect">An action to configure the select menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddMultiStaticSelectMenu(string actionId,
        Action<InputElementBuilder<StaticMultiSelectMenu>> createMultiStaticSelect)
        => AddElement(actionId, createMultiStaticSelect);

    /// <summary>
    /// Adds a multi-select external select menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the select menu</param>
    /// <param name="createMultiExternalSelect">An action to configure the select menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddMultiExternalSelectMenu(string actionId,
        Action<InputElementBuilder<ExternalMultiSelectMenu>> createMultiExternalSelect)
        => AddElement(actionId, createMultiExternalSelect);

    /// <summary>
    /// Adds a multi-select user select menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the select menu</param>
    /// <param name="createMultiUserSelect">An action to configure the select menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddMultiUserSelectMenu(string actionId,
        Action<InputElementBuilder<UserMultiSelectMenu>> createMultiUserSelect)
        => AddElement(actionId, createMultiUserSelect);

    /// <summary>
    /// Adds a multi-select conversation select menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the select menu</param>
    /// <param name="createMultiConversationsSelect">An action to configure the select menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddMultiConversationSelectMenu(string actionId,
        Action<InputElementBuilder<ConversationMultiSelectMenu>> createMultiConversationsSelect)
        => AddElement(actionId, createMultiConversationsSelect);

    /// <summary>
    /// Adds a multi-select channel select menu to the actions block
    /// </summary>
    /// <param name="actionId">The action ID for the select menu</param>
    /// <param name="createMultiChannelSelect">An action to configure the select menu</param>
    /// <returns>The same instance so calls can be chained</returns>
    public ActionsBlockBuilder AddMultiChannelSelectMenu(string actionId,
        Action<InputElementBuilder<ChannelMultiSelectMenu>> createMultiChannelSelect)
        => AddElement(actionId, createMultiChannelSelect);
}