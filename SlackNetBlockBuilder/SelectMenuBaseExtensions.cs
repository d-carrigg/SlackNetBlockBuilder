using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Provides extension methods for configuring various types of select menu elements (<see cref="SelectMenuBase"/> and its derivatives)
/// within an <see cref="InputElementBuilder{TElement}"/>.
/// </summary>
[PublicAPI]
public static class SelectMenuBaseExtensions
{
    /// <summary>
    /// Sets the placeholder text shown on the menu.
    /// Maximum length 150 characters.
    /// </summary>
    /// <typeparam name="TElement">The type of the select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="placeholder">The placeholder text.</param>
    /// <summary>
            /// Sets the placeholder text for a Slack select menu element.
            /// </summary>
            /// <param name="placeholder">The placeholder text to display when no option is selected (maximum 150 characters).</param>
            /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> Placeholder<TElement>(
        this InputElementBuilder<TElement> builder,
        string placeholder) where TElement : SelectMenuBase
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Set(x => x.Placeholder = placeholder);

    /// <summary>
    /// Indicates whether the element will be set to autofocus within the view object.
    /// Only one element can be set to true. Defaults to false.
    /// </summary>
    /// <typeparam name="TElement">The type of the select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="focus">True to focus on load.</param>
    /// <summary>
            /// Sets whether the select menu element should automatically receive focus when the view loads.
            /// </summary>
            /// <param name="focus">If true, the element will be focused on load. Only one element can be focused per view.</param>
            /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> FocusOnLoad<TElement>(
        this InputElementBuilder<TElement> builder,
        bool focus = true) where TElement : SelectMenuBase
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Set(x => x.FocusOnLoad = focus);


    /// <summary>
    /// Adds a standard option to a static select menu.
    /// </summary>
    /// <typeparam name="TElement">The type of the static select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="value">The string value that will be passed to your app when this option is chosen. Maximum length 75 characters.</param>
    /// <param name="text">A plain text object that defines the text shown in the option on the menu. Maximum length 75 characters.</param>
    /// <param name="description">An optional plain text object shown below the <paramref name="text"/> field. Maximum length 75 characters.</param>
    /// <summary>
            /// Adds an option to a static select menu with the specified value, display text, and optional description.
            /// </summary>
            /// <param name="value">The unique value for the option (maximum 75 characters).</param>
            /// <param name="text">The display text for the option (maximum 75 characters).</param>
            /// <param name="description">An optional description for the option (maximum 75 characters).</param>
            /// <returns>The same builder instance for fluent chaining.</returns>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is null.</exception>
    public static InputElementBuilder<TElement> AddOption<TElement>(this InputElementBuilder<TElement> builder,
        string value,
        string text, PlainText? description = null) where TElement : StaticSelectMenuBase =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x =>
            x.Options.Add(new Option { Text = text, Value = value, Description = description }));


    /// <summary>
    /// Adds a pre-defined option group to a static select menu.
    /// </summary>
    /// <typeparam name="TElement">The type of the static select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="label">A plain text label for the group. Maximum length 75 characters.</param>
    /// <param name="options">The list of options within this group.</param>
    /// <summary>
            /// Adds an option group with the specified label and options to a static select menu element.
            /// </summary>
            /// <param name="label">The label for the option group.</param>
            /// <param name="options">The list of options to include in the group.</param>
            /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> AddOptionGroup<TElement>(this InputElementBuilder<TElement> builder,
        string label, IList<Option> options) where TElement : StaticSelectMenuBase =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x =>
            x.OptionGroups.Add(new OptionGroup { Label = label, Options = options }));

    /// <summary>
    /// Adds an option group to a static select menu using a configuration action.
    /// </summary>
    /// <typeparam name="TElement">The type of the static select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="label">A plain text label for the group. Maximum length 75 characters.</param>
    /// <param name="groupBuilder">An action that configures the options within the group using an <see cref="OptionGroupBuilder"/>.</param>
    /// <summary>
    /// Adds an option group with the specified label to the select menu, configuring its options using the provided builder action.
    /// </summary>
    /// <param name="label">The label for the option group.</param>
    /// <param name="groupBuilder">An action to configure the options within the group.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> AddOptionGroup<TElement>(this InputElementBuilder<TElement> builder,
        string label, Action<OptionGroupBuilder> groupBuilder) where TElement : StaticSelectMenuBase
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(groupBuilder);
        var group = new OptionGroup { Label = label };
        groupBuilder(new OptionGroupBuilder(group));
        return builder.Set(x => x.OptionGroups.Add(group));
    }


    // Static Select
    /// <summary>
    /// Sets the initially selected option in a <see cref="StaticSelectMenu"/>.
    /// The matching option is found by comparing the provided <paramref name="value"/> with the <see cref="Option.Value"/> of the options/option groups already added.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="StaticSelectMenu"/>.</param>
    /// <param name="value">The value string of the option to select initially.</param>
    /// <summary>
        /// Sets the initially selected option in a static select menu by matching the provided value to an existing option.
        /// </summary>
        /// <param name="value">The value of the option to select initially.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<StaticSelectMenu> InitialOption(
        this InputElementBuilder<StaticSelectMenu> builder,
        string value) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialOption = x.Options.FirstOrDefault(o => o.Value == value));

    // Static Multi Select
    /// <summary>
    /// Sets the initially selected options in a <see cref="StaticMultiSelectMenu"/> using a selector function.
    /// </summary>
    /// <typeparam name="TElement">The type of the static multi-select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="selector">A function that takes the current list of options and returns the subset to be initially selected.</param>
    /// <summary>
        /// Sets the initially selected options for a static multi-select menu using a selector function.
        /// </summary>
        /// <param name="selector">A function that selects the initial options from the available options.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> InitialOptions<TElement>(
        this InputElementBuilder<TElement> builder,
        Func<IList<Option>, IList<Option>> selector) where TElement : StaticMultiSelectMenu =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialOptions = selector(x.Options));

    /// <summary>
    /// Sets the initially selected options in a <see cref="StaticMultiSelectMenu"/> by their values.
    /// </summary>
    /// <typeparam name="TElement">The type of the static multi-select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="initialOptions">The value strings of the options to select initially.</param>
    /// <summary>
        /// Sets the initially selected options in a static multi-select menu by matching the provided option values.
        /// </summary>
        /// <param name="initialOptions">An array of option values to be initially selected.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> InitialOptions<TElement>(
        this InputElementBuilder<TElement> builder,
        params string[] initialOptions) where TElement : StaticMultiSelectMenu =>
        builder.InitialOptions(options => options.Where(o => initialOptions.Contains(o.Value)).ToList());

    /// <summary>
    /// Sets the initially selected options in a <see cref="StaticMultiSelectMenu"/> by their values.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="StaticMultiSelectMenu"/>.</param>
    /// <param name="initialOptions">The value strings of the options to select initially.</param>
    /// <summary>
            /// Sets the initially selected options in a static multi-select menu by matching the provided option values.
            /// </summary>
            /// <param name="builder">The builder for the static multi-select menu.</param>
            /// <param name="initialOptions">An array of option values to be initially selected.</param>
            /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<StaticMultiSelectMenu> InitialOptions(
        this InputElementBuilder<StaticMultiSelectMenu> builder,
        params string[] initialOptions) =>
        builder.InitialOptions(options =>
            options.Where(o => initialOptions.Contains(o.Value)).ToList());
    
    /// <summary>
    /// Specifies the maximum number of items that can be selected in a <see cref="StaticMultiSelectMenu"/>.
    /// Minimum number is 1.
    /// </summary>
    /// <typeparam name="TElement">The type of the static multi-select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="maxItems">The maximum number of items.</param>
    /// <summary>
        /// Sets the maximum number of selectable items for a static multi-select menu.
        /// </summary>
        /// <param name="maxItems">The maximum number of items that can be selected (minimum 1).</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> MaxSelectedItems<TElement>(
        this InputElementBuilder<TElement> builder,
        int maxItems) where TElement : StaticMultiSelectMenu =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.MaxSelectedItems = maxItems);

  


    // User Select menu
    /// <summary>
    /// Sets the initially selected user in a <see cref="UserSelectMenu"/>.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="UserSelectMenu"/>.</param>
    /// <param name="userId">The user ID to select initially.</param>
    /// <summary>
        /// Sets the initially selected user in the user select menu by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user to pre-select.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<UserSelectMenu> InitialUser(
        this InputElementBuilder<UserSelectMenu> builder,
        string userId) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialUser = userId);


    // External Select menu base
    /// <summary>
    /// Sets the minimum number of characters the user must type before a request is sent to the external data source.
    /// </summary>
    /// <typeparam name="TElement">The type of the external select menu element.</typeparam>
    /// <param name="builder">The builder instance.</param>
    /// <param name="minQueryLength">The minimum query length.</param>
    /// <summary>
            /// Sets the minimum number of characters required before querying the external data source for select menu options.
            /// </summary>
            /// <param name="minQueryLength">The minimum number of characters to trigger the external data source query.</param>
            /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<TElement> MinQueryLength<TElement>(
        this InputElementBuilder<TElement> builder,
        int minQueryLength)
        where TElement : ExternalSelectMenuBase
        => 
            builder is null ? throw new ArgumentNullException(nameof(builder)) :
            builder.Set(x => x.MinQueryLength = minQueryLength);

    // External Select Menu
    /// <summary>
    /// Sets the initially selected option in an <see cref="ExternalSelectMenu"/>.
    /// </summary>
    /// <param name="builder">The builder for an <see cref="ExternalSelectMenu"/>.</param>
    /// <param name="option">The <see cref="Option"/> object to select initially.</param>
    /// <summary>
        /// Sets the initially selected option for the external select menu.
        /// </summary>
        /// <param name="option">The option to pre-select when the menu is displayed.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ExternalSelectMenu> InitialOption(
        this InputElementBuilder<ExternalSelectMenu> builder,
        Option option) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialOption = option);

    // Conversation Select Menu
    /// <summary>
    /// Sets the initially selected conversation in a <see cref="ConversationSelectMenu"/>.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationSelectMenu"/>.</param>
    /// <param name="conversationId">The ID of the conversation (public channel, private channel, DM) to select initially.</param>
    /// <summary>
        /// Sets the initially selected conversation in the conversation select menu by its ID.
        /// </summary>
        /// <param name="conversationId">The ID of the conversation to pre-select.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationSelectMenu> InitialConversation(
        this InputElementBuilder<ConversationSelectMenu> builder,
        string conversationId) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialConversation = conversationId);

    /// <summary>
    /// Pre-populates the select menu with the conversation that the user was viewing when they opened the modal, if available.
    /// Defaults to false.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationSelectMenu"/>.</param>
    /// <param name="defaultToCurrentConversation">True to default to the current conversation.</param>
    /// <summary>
        /// Sets whether the conversation select menu should default to the conversation the user was viewing when opening the modal.
        /// </summary>
        /// <param name="defaultToCurrentConversation">If true, pre-populates the menu with the current conversation; otherwise, does not pre-populate.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationSelectMenu> DefaultToCurrentConversation(
        this InputElementBuilder<ConversationSelectMenu> builder,
        bool defaultToCurrentConversation = true) =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.DefaultToCurrentConversation = defaultToCurrentConversation);

    /// <summary>
    /// Configures the filter for conversations shown in the menu.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationSelectMenu"/>.</param>
    /// <param name="createFilter">An action that configures the <see cref="ConversationFilter"/>.</param>
    /// <summary>
    /// Configures the conversation filter for a conversation select menu using the provided action.
    /// </summary>
    /// <param name="createFilter">An action that modifies the <see cref="ConversationFilter"/> for the menu.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationSelectMenu> Filter(
        this InputElementBuilder<ConversationSelectMenu> builder,
        Action<ConversationFilter> createFilter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(createFilter);
        var filter = builder.Element.Filter ?? new ConversationFilter();
        createFilter(filter);
        return builder.Set(x => x.Filter = filter);
    }

    /// <summary>
    /// Includes specific conversation types in the filter.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationSelectMenu"/>.</param>
    /// <param name="type">The conversation type to include (<see cref="ConversationTypeFilter.Im"/>, <see cref="ConversationTypeFilter.Mpim"/>, <see cref="ConversationTypeFilter.Private"/>, <see cref="ConversationTypeFilter.Public"/>).</param>
    /// <summary>
        /// Adds a conversation type to the filter for a conversation select menu.
        /// </summary>
        /// <param name="type">The conversation type to include in the filter.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationSelectMenu> Include(
        this InputElementBuilder<ConversationSelectMenu> builder,
        ConversationTypeFilter type) =>
        builder.Filter(x => x.Include.Add(type));

    /// <summary>
    /// Excludes external shared channels from the list.
    /// Defaults to false.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationSelectMenu"/>.</param>
    /// <param name="exclude">True to exclude external shared channels.</param>
    /// <summary>
        /// Sets whether to exclude external shared channels from the selectable conversations in the menu.
        /// </summary>
        /// <param name="exclude">If true, external shared channels are excluded; otherwise, they are included. Defaults to true.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationSelectMenu> ExcludeExternalSharedChannels(
        this InputElementBuilder<ConversationSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeExternalSharedChannels = exclude);

    /// <summary>
    /// Excludes bot users from the list of direct message conversations.
    /// Defaults to false.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationSelectMenu"/>.</param>
    /// <param name="exclude">True to exclude bot users.</param>
    /// <summary>
        /// Sets whether to exclude bot users from the selectable conversations in the menu.
        /// </summary>
        /// <param name="exclude">If true, bot users are excluded from direct message conversations. Defaults to true.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationSelectMenu> ExcludeBotUsers(
        this InputElementBuilder<ConversationSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeBotUsers = exclude);

    // Channel Select Menu
    /// <summary>
    /// Sets the initially selected public channel in a <see cref="ChannelSelectMenu"/>.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ChannelSelectMenu"/>.</param>
    /// <param name="channelId">The ID of the public channel to select initially.</param>
    /// <summary>
        /// Sets the initially selected public channel in the channel select menu by its channel ID.
        /// </summary>
        /// <param name="channelId">The ID of the channel to pre-select.</param>
        /// <returns>The same builder instance for method chaining.</returns>
    public static InputElementBuilder<ChannelSelectMenu> InitialChannel(
        this InputElementBuilder<ChannelSelectMenu> builder,
        string channelId) =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialChannel = channelId);

    // Static Multi Select Menu
    
    // external multi select menu
    /// <summary>
    /// Sets the initially selected options in an <see cref="ExternalMultiSelectMenu"/>.
    /// </summary>
    /// <param name="builder">The builder for an <see cref="ExternalMultiSelectMenu"/>.</param>
    /// <param name="initialOptions">A list of <see cref="Option"/> objects to select initially.</param>
    /// <summary>
        /// Sets the initially selected options for an external multi-select menu.
        /// </summary>
        /// <param name="initialOptions">The list of options to pre-select in the menu.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ExternalMultiSelectMenu> InitialOptions(
        this InputElementBuilder<ExternalMultiSelectMenu> builder,
        IList<Option> initialOptions) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialOptions = initialOptions);

    // user multi select menu

    /// <summary>
    /// Sets the initially selected users in a <see cref="UserMultiSelectMenu"/>.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="UserMultiSelectMenu"/>.</param>
    /// <param name="userIds">The user IDs to select initially.</param>
    /// <summary>
        /// Sets the initially selected users in a user multi-select menu by their user IDs.
        /// </summary>
        /// <param name="userIds">The user IDs to pre-select in the menu.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<UserMultiSelectMenu> InitialUsers(
        this InputElementBuilder<UserMultiSelectMenu> builder,
        params string[] userIds) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialUsers = userIds);

    /// <summary>
    /// Specifies the maximum number of items that can be selected in a <see cref="UserMultiSelectMenu"/>.
    /// Minimum number is 1.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="UserMultiSelectMenu"/>.</param>
    /// <param name="maxItems">The maximum number of items.</param>
    /// <summary>
        /// Sets the maximum number of users that can be selected in the user multi-select menu.
        /// </summary>
        /// <param name="maxItems">The maximum number of selectable users (must be at least 1).</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<UserMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<UserMultiSelectMenu> builder,
        int maxItems) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.MaxSelectedItems = maxItems);

    // conversation multi select menu

    /// <summary>
    /// Sets the initially selected conversations in a <see cref="ConversationMultiSelectMenu"/>.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationMultiSelectMenu"/>.</param>
    /// <param name="conversationIds">The IDs of the conversations to select initially.</param>
    /// <summary>
        /// Sets the initially selected conversations in a conversation multi-select menu by their IDs.
        /// </summary>
        /// <param name="conversationIds">The IDs of the conversations to pre-select.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> InitialConversations(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        params string[] conversationIds) =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialConversations = conversationIds);

    /// <summary>
    /// Pre-populates the select menu with the conversation that the user was viewing when they opened the modal, if available.
    /// Defaults to false.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationMultiSelectMenu"/>.</param>
    /// <param name="defaultToCurrentConversation">True to default to the current conversation.</param>
    /// <summary>
        /// Sets whether the conversation multi-select menu should be pre-populated with the conversation the user was viewing when opening the modal.
        /// </summary>
        /// <param name="defaultToCurrentConversation">If true, pre-populates with the current conversation; otherwise, does not.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> DefaultToCurrentConversation(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        bool defaultToCurrentConversation = true) =>
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.DefaultToCurrentConversation = defaultToCurrentConversation);

    /// <summary>
    /// Configures the filter for conversations shown in the menu.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationMultiSelectMenu"/>.</param>
    /// <param name="createFilter">An action that configures the <see cref="ConversationFilter"/>.</param>
    /// <summary>
    /// Configures the conversation filter for a conversation multi-select menu using the specified filter action.
    /// </summary>
    /// <param name="createFilter">An action that configures the <see cref="ConversationFilter"/> for the menu.</param>
    /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> Filter(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        Action<ConversationFilter> createFilter)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(createFilter);
        var filter = builder.Element.Filter ?? new ConversationFilter();
        createFilter(filter);
        return builder.Set(x => x.Filter = filter);
    }

    /// <summary>
    /// Includes specific conversation types in the filter.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationMultiSelectMenu"/>.</param>
    /// <param name="type">The conversation type to include (<see cref="ConversationTypeFilter.Im"/>, <see cref="ConversationTypeFilter.Mpim"/>, <see cref="ConversationTypeFilter.Private"/>, <see cref="ConversationTypeFilter.Public"/>).</param>
    /// <summary>
        /// Adds a conversation type to the filter for a conversation multi-select menu, allowing only conversations of the specified type to be selectable.
        /// </summary>
        /// <param name="type">The conversation type to include in the filter.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> Include(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        ConversationTypeFilter type) =>
        builder.Filter(x => x.Include.Add(type));

    /// <summary>
    /// Excludes external shared channels from the list.
    /// Defaults to false.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationMultiSelectMenu"/>.</param>
    /// <param name="exclude">True to exclude external shared channels.</param>
    /// <summary>
        /// Sets whether to exclude external shared channels from the selectable conversations in the multi-select menu.
        /// </summary>
        /// <param name="exclude">If true, external shared channels are excluded; otherwise, they are included. Defaults to true.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> ExcludeExternalSharedChannels(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeExternalSharedChannels = exclude);

    /// <summary>
    /// Excludes bot users from the list of direct message conversations.
    /// Defaults to false.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationMultiSelectMenu"/>.</param>
    /// <param name="exclude">True to exclude bot users.</param>
    /// <summary>
        /// Sets whether to exclude bot users from the selectable conversations in the multi-select menu.
        /// </summary>
        /// <param name="exclude">If true, bot users are excluded from direct message conversations. Defaults to true.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> ExcludeBotUsers(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        bool exclude = true) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Filter(x => x.ExcludeBotUsers = exclude);

    /// <summary>
    /// Specifies the maximum number of items that can be selected in a <see cref="ConversationMultiSelectMenu"/>.
    /// Minimum number is 1.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ConversationMultiSelectMenu"/>.</param>
    /// <param name="maxItems">The maximum number of items.</param>
    /// <summary>
        /// Sets the maximum number of selectable conversations in the multi-select menu.
        /// </summary>
        /// <param name="maxItems">The maximum number of conversations that can be selected (minimum 1).</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        int maxItems) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.MaxSelectedItems = maxItems);

    // channel multi select menu
    /// <summary>
    /// Sets the initially selected public channels in a <see cref="ChannelMultiSelectMenu"/>.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ChannelMultiSelectMenu"/>.</param>
    /// <param name="channelIds">The IDs of the public channels to select initially.</param>
    /// <summary>
        /// Sets the initially selected public channels in a channel multi-select menu by their IDs.
        /// </summary>
        /// <param name="channelIds">The IDs of the channels to pre-select.</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ChannelMultiSelectMenu> InitialChannels(
        this InputElementBuilder<ChannelMultiSelectMenu> builder,
        params string[] channelIds) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.InitialChannels = channelIds);

    /// <summary>
    /// Specifies the maximum number of items that can be selected in a <see cref="ChannelMultiSelectMenu"/>.
    /// Minimum number is 1.
    /// </summary>
    /// <param name="builder">The builder for a <see cref="ChannelMultiSelectMenu"/>.</param>
    /// <param name="maxItems">The maximum number of items.</param>
    /// <summary>
        /// Sets the maximum number of selectable channels in a channel multi-select menu.
        /// </summary>
        /// <param name="maxItems">The maximum number of channels that can be selected (must be at least 1).</param>
        /// <returns>The same builder instance so calls can be chained.</returns>
    public static InputElementBuilder<ChannelMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<ChannelMultiSelectMenu> builder,
        int maxItems) => 
        builder is null ? throw new ArgumentNullException(nameof(builder)) :
        builder.Set(x => x.MaxSelectedItems = maxItems);
}