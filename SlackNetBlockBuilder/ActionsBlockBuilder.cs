using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Provides a fluent interface for building <see cref="ActionsBlock"/> instances.
/// An Actions block is a container for interactive elements like buttons, select menus, and date pickers.
/// </summary>
[PublicAPI]
public sealed class ActionsBlockBuilder
{

    /// <summary>
    /// The maximum number of elements allowed in an Actions block.
    /// </summary>
    public const int MaxElements = 25;
    
    /// <summary>
    /// The maximum length allowed for a block ID.
    /// </summary>
    public const int MaxBlockIdLength = 255;
    
    /// <summary>
    /// The maximum length allowed for an action ID.
    /// </summary>
    public const int MaxActionIdLength = 255;
    
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionsBlockBuilder"/> class.
    /// </summary>
    private ActionsBlockBuilder()
    {
        _element = new();
    }
    /// <summary>
    /// Creates a new <see cref="ActionsBlockBuilder"/> instance.
    /// </summary>
    /// <summary>
/// Creates a new instance of <see cref="ActionsBlockBuilder"/> for constructing an actions block.
/// </summary>
/// <returns>A new <see cref="ActionsBlockBuilder"/>.</returns>
    public static ActionsBlockBuilder Create() => new();
    /// <summary>
    /// Builds the <see cref="ActionsBlock"/> instance.
    /// </summary>
    /// <returns>The constructed <see cref="ActionsBlock"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the block contains more than <see cref="MaxElements"/> elements
    /// or if the <see cref="Block.BlockId"/> exceeds <see cref="MaxBlockIdLength"/> characters.
    /// <summary>
    /// Finalizes and returns the constructed <see cref="ActionsBlock"/> instance.
    /// </summary>
    /// <returns>The configured <see cref="ActionsBlock"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the block contains more than <see cref="MaxElements"/> elements, the block ID exceeds <see cref="MaxBlockIdLength"/> characters, or any element's action ID exceeds <see cref="MaxActionIdLength"/> characters.
    /// </exception>
    public ActionsBlock Build()
    {
        if (_element.Elements.Count > MaxElements)
        {
            throw new InvalidOperationException($"An actions block can only contain up to {MaxElements} elements");
        }
        
        if(_element.BlockId?.Length > MaxBlockIdLength)
        {
            throw new InvalidOperationException($"The block id can only be up to {MaxBlockIdLength} characters long");
        }
        
        foreach (var element in _element.Elements)
        {
            if (element.ActionId?.Length > MaxActionIdLength)
            {
                throw new InvalidOperationException($"The action id can only be up to {MaxActionIdLength} characters long");
            }
        }
        
        return _element;
    }

    private readonly ActionsBlock _element;


    /// <summary>
    /// Sets a unique identifier for this block.
    /// </summary>
    /// <param name="blockId">A unique string identifier for the block. Maximum length is 255 characters.</param>
    /// <summary>
    /// Sets the unique identifier for the actions block.
    /// </summary>
    /// <param name="blockId">A string that uniquely identifies the block within the Slack message.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public ActionsBlockBuilder WithBlockId(string blockId)
    {
        _element.BlockId = blockId;
        return this;
    }

    /// <summary>
    /// Adds a pre-instantiated action element to the block, configured via a builder action.
    /// Primarily intended for internal use or advanced scenarios.
    /// </summary>
    /// <typeparam name="TElement">The type of the action element.</typeparam>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="element">The action element instance to add.</param>
    /// <param name="createElement">An action that configures the element using its specific builder.</param>
    /// <summary>
    /// Adds an action element to the actions block, configuring it with the provided builder action.
    /// </summary>
    /// <typeparam name="TElement">The type of action element to add.</typeparam>
    /// <param name="actionId">The unique action identifier for the element.</param>
    /// <param name="element">The action element instance to configure and add.</param>
    /// <param name="createElement">A delegate to configure the element using an <see cref="ActionElementBuilder{TElement}"/>.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public ActionsBlockBuilder AddElement<TElement>(string actionId, TElement element,
        Action<ActionElementBuilder<TElement>> createElement) where TElement : ActionElement
    {
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(createElement);
        element.ActionId = actionId;
        createElement(new ActionElementBuilder<TElement>(element));
        _element.Elements.Add(element);

        return this;
    }

    /// <summary>
    /// Adds a new action element to the block, configured via a builder action.
    /// Primarily intended for internal use or advanced scenarios.
    /// </summary>
    /// <typeparam name="TElement">The type of the action element. Must have a parameterless constructor.</typeparam>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createElement">An action that configures the element using its specific builder.</param>
    /// <summary>
    /// Adds a new action element of the specified type to the actions block, configuring it with the provided delegate.
    /// </summary>
    /// <typeparam name="TElement">The type of action element to add.</typeparam>
    /// <param name="actionId">The unique identifier for the action element.</param>
    /// <param name="createElement">A delegate to configure the new element.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public ActionsBlockBuilder AddElement<TElement>(string actionId,
        Action<ActionElementBuilder<TElement>> createElement) where TElement : ActionElement, new()
    {
        ArgumentNullException.ThrowIfNull(createElement);
        var element = new TElement
        {
            ActionId = actionId
        };
        createElement(new ActionElementBuilder<TElement>(element));
        _element.Elements.Add(element);

        return this;
    }
    
    /// <summary>
    /// Adds a new input block element to the block, configured via a builder action.
    /// Primarily intended for internal use or advanced scenarios.
    /// </summary>
    /// <typeparam name="TElement">The type of the input block element. Must have a parameterless constructor.</typeparam>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createElement">An action that configures the element using its specific builder.</param>
    /// <summary>
    /// Adds a new input block element with the specified action ID and configuration to the actions block.
    /// </summary>
    /// <typeparam name="TElement">The type of input block element to add.</typeparam>
    /// <param name="actionId">The unique identifier for the action element.</param>
    /// <param name="createElement">A delegate to configure the input element.</param>
    /// <returns>The same builder instance for method chaining.</returns>
    public ActionsBlockBuilder AddElement<TElement>(string actionId,
        Action<InputElementBuilder<TElement>> createElement) where TElement : ActionElement, IInputBlockElement, new()
    {
        ArgumentNullException.ThrowIfNull(createElement);
        var element = new TElement { ActionId = actionId };
        createElement(new InputElementBuilder<TElement>(element));
        _element.Elements.Add(element);

        return this;
    }
    
    /// <summary>
    /// Adds a <see cref="Button"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createButton">An action that configures the button using an <see cref="ActionElementBuilder{Button}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="Button"/>
    /// <summary>
        /// Adds a button element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the button action.</param>
        /// <param name="createButton">A delegate to configure the button element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for chaining.</returns>
        /// <seealso cref="ActionElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddButton(string actionId, Action<ActionElementBuilder<Button>> createButton)
        => AddElement(actionId, createButton);

    /// <summary>
    /// Adds a <see cref="CheckboxGroup"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createCheckbox">An action that configures the checkbox group using an <see cref="InputElementBuilder{CheckboxGroup}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="CheckboxGroup"/>
    /// <summary>
        /// Adds a checkbox group element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the checkbox group action.</param>
        /// <param name="createCheckbox">A delegate to configure the <see cref="CheckboxGroup"/> element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for chaining.</returns>
    public ActionsBlockBuilder AddCheckboxGroup(string actionId,
        Action<InputElementBuilder<CheckboxGroup>> createCheckbox)
        => AddElement(actionId, createCheckbox);

    /// <summary>
    /// Adds a <see cref="DatePicker"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createDatePicker">An action that configures the date picker using an <see cref="InputElementBuilder{DatePicker}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="DatePicker"/>
    /// <summary>
        /// Adds a date picker element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the date picker action.</param>
        /// <param name="createDatePicker">A delegate to configure the date picker element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for method chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddDatePicker(string actionId, Action<InputElementBuilder<DatePicker>> createDatePicker)
        => AddElement(actionId, createDatePicker);

    /// <summary>
    /// Adds a <see cref="TimePicker"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createTimePicker">An action that configures the time picker using an <see cref="InputElementBuilder{TimePicker}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="TimePicker"/>
    /// <summary>
        /// Adds a time picker element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the time picker element.</param>
        /// <param name="createTimePicker">A delegate to configure the time picker element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddTimePicker(string actionId, Action<InputElementBuilder<TimePicker>> createTimePicker)
        => AddElement(actionId, createTimePicker);

    /// <summary>
    /// Adds a <see cref="DateTimePicker"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createDateTimePicker">An action that configures the date time picker using an <see cref="InputElementBuilder{DateTimePicker}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="DateTimePicker"/>
    /// <summary>
        /// Adds a date and time picker element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the date and time picker action.</param>
        /// <param name="createDateTimePicker">A delegate to configure the <see cref="DateTimePicker"/> element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddDateTimePicker(string actionId,
        Action<InputElementBuilder<DateTimePicker>> createDateTimePicker)
        => AddElement(actionId, createDateTimePicker);

    /// <summary>
    /// Adds an <see cref="OverflowMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createOverflowMenu">An action that configures the overflow menu using an <see cref="ActionElementBuilder{OverflowMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="OverflowMenu"/>
    /// <summary>
        /// Adds an overflow menu element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the overflow menu action.</param>
        /// <param name="createOverflowMenu">A delegate to configure the overflow menu element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for method chaining.</returns>
        /// <seealso cref="ActionElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddOverflowMenu(string actionId,
        Action<ActionElementBuilder<OverflowMenu>> createOverflowMenu) => AddElement(actionId, createOverflowMenu);

    /// <summary>
    /// Adds a <see cref="RadioButtonGroup"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createRadioButtonGroup">An action that configures the radio button group using an <see cref="InputElementBuilder{RadioButtonGroup}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="RadioButtonGroup"/>
    /// <summary>
        /// Adds a radio button group element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the radio button group action.</param>
        /// <param name="createRadioButtonGroup">A delegate to configure the radio button group element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddRadioButtonGroup(string actionId,
        Action<InputElementBuilder<RadioButtonGroup>> createRadioButtonGroup)
        => AddElement(actionId, createRadioButtonGroup);

    /// <summary>
    /// Adds a <see cref="StaticSelectMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createStaticSelect">An action that configures the static select menu using an <see cref="InputElementBuilder{StaticSelectMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="StaticSelectMenu"/>
    /// <summary>
        /// Adds a static select menu element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the select menu action.</param>
        /// <param name="createStaticSelect">A delegate to configure the static select menu element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for method chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddStaticSelectMenu(string actionId,
        Action<InputElementBuilder<StaticSelectMenu>> createStaticSelect)
        => AddElement(actionId, createStaticSelect);

    /// <summary>
    /// Adds an <see cref="ExternalSelectMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createExternalSelect">An action that configures the external select menu using an <see cref="InputElementBuilder{ExternalSelectMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="ExternalSelectMenu"/>
    /// <summary>
        /// Adds an external select menu element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the action triggered when the select menu is used.</param>
        /// <param name="createExternalSelect">A delegate to configure the external select menu element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddExternalSelectMenu(string actionId,
        Action<InputElementBuilder<ExternalSelectMenu>> createExternalSelect)
        => AddElement(actionId, createExternalSelect);

    /// <summary>
    /// Adds a <see cref="UserSelectMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createUserSelect">An action that configures the user select menu using an <see cref="InputElementBuilder{UserSelectMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="UserSelectMenu"/>
    /// <summary>
        /// Adds a user select menu element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the user select menu action.</param>
        /// <param name="createUserSelect">A delegate to configure the <see cref="UserSelectMenu"/> element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for method chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddUserSelectMenu(string actionId,
        Action<InputElementBuilder<UserSelectMenu>> createUserSelect)
        => AddElement(actionId, createUserSelect);

    /// <summary>
    /// Adds a <see cref="ConversationSelectMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createConversationsSelect">An action that configures the conversation select menu using an <see cref="InputElementBuilder{ConversationSelectMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="ConversationSelectMenu"/>
    /// <summary>
        /// Adds a conversation select menu element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the action triggered when the menu is used.</param>
        /// <param name="createConversationsSelect">A delegate to configure the conversation select menu element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for method chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddConversationSelectMenu(string actionId,
        Action<InputElementBuilder<ConversationSelectMenu>> createConversationsSelect)
        => AddElement(actionId, createConversationsSelect);

    /// <summary>
    /// Adds a <see cref="ChannelSelectMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createChannelSelect">An action that configures the channel select menu using an <see cref="InputElementBuilder{ChannelSelectMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="ChannelSelectMenu"/>
    /// <summary>
        /// Adds a channel select menu element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the channel select menu action.</param>
        /// <param name="createChannelSelect">A delegate to configure the <see cref="ChannelSelectMenu"/> element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddChannelSelectMenu(string actionId,
        Action<InputElementBuilder<ChannelSelectMenu>> createChannelSelect)
        => AddElement(actionId, createChannelSelect);

    /// <summary>
    /// Adds a <see cref="StaticMultiSelectMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createMultiStaticSelect">An action that configures the multi-static select menu using an <see cref="InputElementBuilder{StaticMultiSelectMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="StaticMultiSelectMenu"/>
    /// <summary>
        /// Adds a multi-select static menu element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the action triggered when the menu is used.</param>
        /// <param name="createMultiStaticSelect">A delegate to configure the multi-select static menu element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for method chaining.</returns>
    public ActionsBlockBuilder AddMultiStaticSelectMenu(string actionId,
        Action<InputElementBuilder<StaticMultiSelectMenu>> createMultiStaticSelect)
        => AddElement(actionId, createMultiStaticSelect);

    /// <summary>
    /// Adds an <see cref="ExternalMultiSelectMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createMultiExternalSelect">An action that configures the multi-external select menu using an <see cref="InputElementBuilder{ExternalMultiSelectMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="ExternalMultiSelectMenu"/>
    /// <summary>
        /// Adds an external multi-select menu element to the actions block.
        /// </summary>
        /// <param name="actionId">A unique identifier for the action triggered when a selection is made.</param>
        /// <param name="createMultiExternalSelect">A delegate to configure the external multi-select menu element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for method chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddMultiExternalSelectMenu(string actionId,
        Action<InputElementBuilder<ExternalMultiSelectMenu>> createMultiExternalSelect)
        => AddElement(actionId, createMultiExternalSelect);

    /// <summary>
    /// Adds a <see cref="UserMultiSelectMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createMultiUserSelect">An action that configures the multi-user select menu using an <see cref="InputElementBuilder{UserMultiSelectMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="UserMultiSelectMenu"/>
    /// <summary>
        /// Adds a multi-user select menu element to the actions block with the specified action ID and configuration.
        /// </summary>
        /// <param name="actionId">A unique identifier for the action triggered when a user interacts with the menu.</param>
        /// <param name="createMultiUserSelect">A delegate to configure the multi-user select menu element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for method chaining.</returns>
    public ActionsBlockBuilder AddMultiUserSelectMenu(string actionId,
        Action<InputElementBuilder<UserMultiSelectMenu>> createMultiUserSelect)
        => AddElement(actionId, createMultiUserSelect);

    /// <summary>
    /// Adds a <see cref="ConversationMultiSelectMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createMultiConversationsSelect">An action that configures the multi-conversation select menu using an <see cref="InputElementBuilder{ConversationMultiSelectMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="ConversationMultiSelectMenu"/>
    /// <summary>
        /// Adds a multi-conversation select menu element to the actions block.
        /// </summary>
        /// <param name="actionId">A unique identifier for this action within the block.</param>
        /// <param name="createMultiConversationsSelect">A delegate to configure the multi-conversation select menu element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for chaining.</returns>
    public ActionsBlockBuilder AddMultiConversationSelectMenu(string actionId,
        Action<InputElementBuilder<ConversationMultiSelectMenu>> createMultiConversationsSelect)
        => AddElement(actionId, createMultiConversationsSelect);

    /// <summary>
    /// Adds a <see cref="ChannelMultiSelectMenu"/> element to the block.
    /// </summary>
    /// <param name="actionId">An identifier for this action. Should be unique. Max length 255.</param>
    /// <param name="createMultiChannelSelect">An action that configures the multi-channel select menu using an <see cref="InputElementBuilder{ChannelMultiSelectMenu}"/>.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    /// <seealso cref="ChannelMultiSelectMenu"/>
    /// <summary>
        /// Adds a multi-channel select menu element to the actions block.
        /// </summary>
        /// <param name="actionId">A unique identifier for the action triggered when a selection is made.</param>
        /// <param name="createMultiChannelSelect">A delegate to configure the multi-channel select menu element.</param>
        /// <returns>The current <see cref="ActionsBlockBuilder"/> instance for method chaining.</returns>
        /// <seealso cref="InputElementBuilder{TElement}"/>
    public ActionsBlockBuilder AddMultiChannelSelectMenu(string actionId,
        Action<InputElementBuilder<ChannelMultiSelectMenu>> createMultiChannelSelect)
        => AddElement(actionId, createMultiChannelSelect);


    
}