using JetBrains.Annotations;

namespace SlackNet.Blocks;

/// <summary>
/// Extension methods for working with select menu elements
/// </summary>
[PublicAPI]
public static class SelectMenuBaseExtensions
{
    /// <summary>
    /// Sets the placeholder text for a select menu
    /// </summary>
    /// <typeparam name="TElemenet">The type of select menu</typeparam>
    /// <param name="builder">The select menu builder</param>
    /// <param name="placeholder">The placeholder text to display</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TElemenet> Placeholder<TElemenet>(
        this InputElementBuilder<TElemenet> builder,
        string placeholder) where TElemenet : SelectMenuBase
        => builder.Set(x => x.Placeholder = placeholder);

    /// <summary>
    /// Sets whether the select menu should be focused when the view is opened
    /// </summary>
    /// <typeparam name="TElemenet">The type of select menu</typeparam>
    /// <param name="builder">The select menu builder</param>
    /// <param name="focus">Whether to focus the element</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TElemenet> FocusOnLoad<TElemenet>(
        this InputElementBuilder<TElemenet> builder,
        bool focus = true) where TElemenet : SelectMenuBase
        => builder.Set(x => x.FocusOnLoad = focus);

    /// <summary>
    /// Adds an option to a static select menu
    /// </summary>
    /// <typeparam name="TElement">The type of select menu</typeparam>
    /// <param name="builder">The select menu builder</param>
    /// <param name="value">The value to be sent to your app when the option is selected</param>
    /// <param name="text">The text to display for the option</param>
    /// <param name="description">Optional description for the option</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TElement> AddOption<TElement>(this InputElementBuilder<TElement> builder,
        string value,
        string text, PlainText description = null) where TElement : StaticSelectMenuBase =>
        builder.Set(x =>
            x.Options.Add(new Option { Text = text, Value = value, Description = description }));

    /// <summary>
    /// Adds an option group to a static select menu
    /// </summary>
    /// <typeparam name="TElement">The type of select menu</typeparam>
    /// <param name="builder">The select menu builder</param>
    /// <param name="label">The label for the option group</param>
    /// <param name="options">The options to include in the group</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TElement> AddOptionGroup<TElement>(this InputElementBuilder<TElement> builder,
        string label, IList<Option> options) where TElement : StaticSelectMenuBase =>
        builder.Set(x =>
            x.OptionGroups.Add(new OptionGroup { Label = label, Options = options }));

    /// <summary>
    /// Adds an option group to a static select menu
    /// </summary>
    /// <typeparam name="TElement">The type of select menu</typeparam>
    /// <param name="builder">The select menu builder</param>
    /// <param name="label">The label for the option group</param>
    /// <param name="groupBuilder">An action which configures the option group</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TElement> AddOptionGroup<TElement>(this InputElementBuilder<TElement> builder,
        string label, Action<OptionGroupBuilder> groupBuilder) where TElement : StaticSelectMenuBase
    {
        var group = new OptionGroup { Label = label };
        groupBuilder(new OptionGroupBuilder(group));
        return builder.Set(x => x.OptionGroups.Add(group));
    }

    // Static Select

    /// <summary>
    /// Sets the initially selected option in a static select menu
    /// </summary>
    /// <param name="builder">The static select menu builder</param>
    /// <param name="value">The value of the option to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<StaticSelectMenu> InitialOption(
        this InputElementBuilder<StaticSelectMenu> builder,
        string value) => builder.Set(x => x.InitialOption = x.Options.FirstOrDefault(o => o.Value == value));

    // Static Multi Select

    /// <summary>
    /// Sets the initially selected options in a static multi-select menu
    /// </summary>
    /// <param name="builder">The static multi-select menu builder</param>
    /// <param name="selector">A function that selects which options should be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<StaticMultiSelectMenu> InitialOptions(
        this InputElementBuilder<StaticMultiSelectMenu> builder,
        Func<IList<Option>, IList<Option>> selector)  => builder.Set(x => x.InitialOptions = selector(x.Options).ToList());

    /// <summary>
    /// Sets the maximum number of items that can be selected in a static multi-select menu
    /// </summary>
    /// <param name="builder">The static multi-select menu builder</param>
    /// <param name="maxSelectedItems">The maximum number of items</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<StaticMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<StaticMultiSelectMenu> builder,
        int maxSelectedItems) => builder.Set(x => x.MaxSelectedItems = maxSelectedItems);

    /// <summary>
    /// Sets the initially selected options in a static multi-select menu by their values
    /// </summary>
    /// <param name="builder">The static multi-select menu builder</param>
    /// <param name="initialOptions">The values of the options to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<StaticMultiSelectMenu> InitialOptions(
        this InputElementBuilder<StaticMultiSelectMenu> builder,
        params string[] initialOptions) =>
        builder.InitialOptions(options => options.Where(o => initialOptions.Contains(o.Value)).ToList());

    // User Select menu

    /// <summary>
    /// Sets the initially selected user in a user select menu
    /// </summary>
    /// <param name="builder">The user select menu builder</param>
    /// <param name="userId">The ID of the user to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<UserSelectMenu> InitialUser(
        this InputElementBuilder<UserSelectMenu> builder,
        string userId) => builder.Set(x => x.InitialUser = userId);

    // External Select menu base

    /// <summary>
    /// Sets the minimum query length for an external select menu
    /// </summary>
    /// <typeparam name="TElement">The type of external select menu</typeparam>
    /// <param name="builder">The external select menu builder</param>
    /// <param name="minQueryLength">The minimum query length</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TElement> MinQueryLength<TElement>(
        this InputElementBuilder<TElement> builder,
        int minQueryLength)
        where TElement : ExternalSelectMenuBase
        => builder.Set(x => x.MinQueryLength = minQueryLength);

    // External Select menu

    /// <summary>
    /// Sets the initially selected option in an external select menu
    /// </summary>
    /// <param name="builder">The external select menu builder</param>
    /// <param name="initialOption">The option to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ExternalSelectMenu> InitialOption(
        this InputElementBuilder<ExternalSelectMenu> builder,
        Option initialOption) => builder.Set(x => x.InitialOption = initialOption);

    // Conversation Select menu

    /// <summary>
    /// Sets the initially selected conversation in a conversation select menu
    /// </summary>
    /// <param name="builder">The conversation select menu builder</param>
    /// <param name="conversationId">The ID of the conversation to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationSelectMenu> InitialConversation(
        this InputElementBuilder<ConversationSelectMenu> builder,
        string conversationId) => builder.Set(x => x.InitialConversation = conversationId);

    /// <summary>
    /// Sets whether the conversation select menu should default to the current conversation
    /// </summary>
    /// <param name="builder">The conversation select menu builder</param>
    /// <param name="defaultToCurrentConversation">Whether to default to the current conversation</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationSelectMenu> DefaultToCurrentConversation(
        this InputElementBuilder<ConversationSelectMenu> builder,
        bool defaultToCurrentConversation = true) =>
        builder.Set(x => x.DefaultToCurrentConversation = defaultToCurrentConversation);

    /// <summary>
    /// Configures the filter for a conversation select menu
    /// </summary>
    /// <param name="builder">The conversation select menu builder</param>
    /// <param name="createFilter">An action which configures the filter</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationSelectMenu> Filter(
        this InputElementBuilder<ConversationSelectMenu> builder,
        Action<ConversationFilter> createFilter)
    {
        var filter = builder.Element.Filter ?? new ConversationFilter();
        createFilter(filter);
        return builder.Set(x => x.Filter = filter);
    }

    /// <summary>
    /// Includes a conversation type in the filter for a conversation select menu
    /// </summary>
    /// <param name="builder">The conversation select menu builder</param>
    /// <param name="type">The type of conversation to include</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationSelectMenu> Include(
        this InputElementBuilder<ConversationSelectMenu> builder,
        ConversationTypeFilter type) =>
        builder.Filter(x => x.Include.Add(type));

    /// <summary>
    /// Sets whether to exclude external shared channels from a conversation select menu
    /// </summary>
    /// <param name="builder">The conversation select menu builder</param>
    /// <param name="exclude">Whether to exclude external shared channels</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationSelectMenu> ExcludeExternalSharedChannels(
        this InputElementBuilder<ConversationSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeExternalSharedChannels = exclude);

    /// <summary>
    /// Sets whether to exclude bot users from a conversation select menu
    /// </summary>
    /// <param name="builder">The conversation select menu builder</param>
    /// <param name="exclude">Whether to exclude bot users</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationSelectMenu> ExcludeBotUsers(
        this InputElementBuilder<ConversationSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeBotUsers = exclude);

    // Channel Select menu

    /// <summary>
    /// Sets the initially selected channel in a channel select menu
    /// </summary>
    /// <param name="builder">The channel select menu builder</param>
    /// <param name="channelId">The ID of the channel to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ChannelSelectMenu> InitialChannel(
        this InputElementBuilder<ChannelSelectMenu> builder,
        string channelId) => builder.Set(x => x.InitialChannel = channelId);

    // Static Multi Select menu
    public static InputElementBuilder<TElement> InitialOptions<TElement>(
        this InputElementBuilder<TElement> builder,
        Func<IList<Option>, IList<Option>> selector) where TElement : StaticMultiSelectMenu =>
        builder.Set(x => x.InitialOptions = selector(x.Options));

    /// <summary>
    /// Sets the initially selected options in a static multi-select menu by their values
    /// </summary>
    /// <typeparam name="TElement">The type of static multi-select menu</typeparam>
    /// <param name="builder">The static multi-select menu builder</param>
    /// <param name="initialOptions">The values of the options to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TElement> InitialOptions<TElement>(
        this InputElementBuilder<TElement> builder,
        params string[] initialOptions) where TElement : StaticMultiSelectMenu =>
        builder.InitialOptions(options => options.Where(o => initialOptions.Contains(o.Value)).ToList());

    /// <summary>
    /// Sets the maximum number of items that can be selected in a static multi-select menu
    /// </summary>
    /// <typeparam name="TElement">The type of static multi-select menu</typeparam>
    /// <param name="builder">The static multi-select menu builder</param>
    /// <param name="maxItems">The maximum number of items</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<TElement> MaxSelectedItems<TElement>(
        this InputElementBuilder<TElement> builder,
        int maxItems) where TElement : StaticMultiSelectMenu =>
        builder.Set(x => x.MaxSelectedItems = maxItems);

    // external multi select menu

    /// <summary>
    /// Sets the initially selected options in an external multi-select menu
    /// </summary>
    /// <param name="builder">The external multi-select menu builder</param>
    /// <param name="initialOptions">The options to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ExternalMultiSelectMenu> InitialOptions(
        this InputElementBuilder<ExternalMultiSelectMenu> builder,
        IList<Option> initialOptions) => 
        builder.Set(x => x.InitialOptions = initialOptions);

    // user multi select menu

    /// <summary>
    /// Sets the initially selected users in a user multi-select menu
    /// </summary>
    /// <param name="builder">The user multi-select menu builder</param>
    /// <param name="userIds">The IDs of the users to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<UserMultiSelectMenu> InitialUsers(
        this InputElementBuilder<UserMultiSelectMenu> builder,
        params string[] userIds) => builder.Set(x => x.InitialUsers = userIds);

    /// <summary>
    /// Sets the maximum number of users that can be selected in a user multi-select menu
    /// </summary>
    /// <param name="builder">The user multi-select menu builder</param>
    /// <param name="maxItems">The maximum number of users</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<UserMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<UserMultiSelectMenu> builder,
        int maxItems) => builder.Set(x => x.MaxSelectedItems = maxItems);

    // conversation multi select menu

    /// <summary>
    /// Sets the initially selected conversations in a conversation multi-select menu
    /// </summary>
    /// <param name="builder">The conversation multi-select menu builder</param>
    /// <param name="conversationIds">The IDs of the conversations to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> InitialConversations(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        params string[] conversationIds) => builder.Set(x => x.InitialConversations = conversationIds);

    /// <summary>
    /// Sets whether the conversation multi-select menu should default to the current conversation
    /// </summary>
    /// <param name="builder">The conversation multi-select menu builder</param>
    /// <param name="defaultToCurrentConversation">Whether to default to the current conversation</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> DefaultToCurrentConversation(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        bool defaultToCurrentConversation = true) =>
        builder.Set(x => x.DefaultToCurrentConversation = defaultToCurrentConversation);

    /// <summary>
    /// Configures the filter for a conversation multi-select menu
    /// </summary>
    /// <param name="builder">The conversation multi-select menu builder</param>
    /// <param name="createFilter">An action which configures the filter</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> Filter(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        Action<ConversationFilter> createFilter)
    {
        var filter = builder.Element.Filter ?? new ConversationFilter();
        createFilter(filter);
        return builder.Set(x => x.Filter = filter);
    }

    /// <summary>
    /// Includes a conversation type in the filter for a conversation multi-select menu
    /// </summary>
    /// <param name="builder">The conversation multi-select menu builder</param>
    /// <param name="type">The type of conversation to include</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> Include(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        ConversationTypeFilter type) =>
        builder.Filter(x => x.Include.Add(type));

    /// <summary>
    /// Sets whether to exclude external shared channels from a conversation multi-select menu
    /// </summary>
    /// <param name="builder">The conversation multi-select menu builder</param>
    /// <param name="exclude">Whether to exclude external shared channels</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> ExcludeExternalSharedChannels(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeExternalSharedChannels = exclude);

    /// <summary>
    /// Sets whether to exclude bot users from a conversation multi-select menu
    /// </summary>
    /// <param name="builder">The conversation multi-select menu builder</param>
    /// <param name="exclude">Whether to exclude bot users</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> ExcludeBotUsers(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeBotUsers = exclude);

    /// <summary>
    /// Sets the maximum number of conversations that can be selected in a conversation multi-select menu
    /// </summary>
    /// <param name="builder">The conversation multi-select menu builder</param>
    /// <param name="maxItems">The maximum number of conversations</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ConversationMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        int maxItems) => builder.Set(x => x.MaxSelectedItems = maxItems);

    // channel multi select menu

    /// <summary>
    /// Sets the initially selected channels in a channel multi-select menu
    /// </summary>
    /// <param name="builder">The channel multi-select menu builder</param>
    /// <param name="channelIds">The IDs of the channels to be initially selected</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ChannelMultiSelectMenu> InitialChannels(
        this InputElementBuilder<ChannelMultiSelectMenu> builder,
        params string[] channelIds) => builder.Set(x => x.InitialChannels = channelIds);

    /// <summary>
    /// Sets the maximum number of channels that can be selected in a channel multi-select menu
    /// </summary>
    /// <param name="builder">The channel multi-select menu builder</param>
    /// <param name="maxItems">The maximum number of channels</param>
    /// <returns>The same instance so calls can be chained</returns>
    public static InputElementBuilder<ChannelMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<ChannelMultiSelectMenu> builder,
        int maxItems) => builder.Set(x => x.MaxSelectedItems = maxItems);
}