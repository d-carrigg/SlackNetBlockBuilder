using JetBrains.Annotations;
using SlackNet.Blocks;

namespace SlackNetBlockBuilder;

[PublicAPI]
public static class SelectMenuBaseExtensions
{
    public static InputElementBuilder<TElemenet> Placeholder<TElemenet>(
        this InputElementBuilder<TElemenet> builder,
        string placeholder) where TElemenet : SelectMenuBase
        => builder.Set(x => x.Placeholder = placeholder);

    public static InputElementBuilder<TElemenet> FocusOnLoad<TElemenet>(
        this InputElementBuilder<TElemenet> builder,
        bool focus = true) where TElemenet : SelectMenuBase
        => builder.Set(x => x.FocusOnLoad = focus);


    public static InputElementBuilder<TElement> AddOption<TElement>(this InputElementBuilder<TElement> builder,
        string value,
        string text, PlainText description = null) where TElement : StaticSelectMenuBase =>
        builder.Set(x =>
            x.Options.Add(new Option { Text = text, Value = value, Description = description }));


    public static InputElementBuilder<TElement> AddOptionGroup<TElement>(this InputElementBuilder<TElement> builder,
        string label, IList<Option> options) where TElement : StaticSelectMenuBase =>
        builder.Set(x =>
            x.OptionGroups.Add(new OptionGroup { Label = label, Options = options }));

    public static InputElementBuilder<TElement> AddOptionGroup<TElement>(this InputElementBuilder<TElement> builder,
        string label, Action<OptionGroupBuilder> groupBuilder) where TElement : StaticSelectMenuBase
    {
        var group = new OptionGroup { Label = label };
        groupBuilder(new OptionGroupBuilder(group));
        return builder.Set(x => x.OptionGroups.Add(group));
    }


    // Static Select
    public static InputElementBuilder<StaticSelectMenu> InitialOption(
        this InputElementBuilder<StaticSelectMenu> builder,
        string value) => builder.Set(x => x.InitialOption = x.Options.FirstOrDefault(o => o.Value == value));

    // Static Multi Select
    public static InputElementBuilder<StaticMultiSelectMenu> InitialOptions(
        this InputElementBuilder<StaticMultiSelectMenu> builder,
        Func<IList<Option>, IList<Option>> selector) => builder.Set(x => x.InitialOptions = selector(x.Options).ToList());

    public static InputElementBuilder<StaticMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<StaticMultiSelectMenu> builder,
        int maxSelectedItems) => builder.Set(x => x.MaxSelectedItems = maxSelectedItems);

    public static InputElementBuilder<StaticMultiSelectMenu> InitialOptions(
        this InputElementBuilder<StaticMultiSelectMenu> builder,
        params string[] initialOptions) =>
        builder.InitialOptions(options => options.Where(o => initialOptions.Contains(o.Value)).ToList());


    // User Select menu
    public static InputElementBuilder<UserSelectMenu> InitialUser(
        this InputElementBuilder<UserSelectMenu> builder,
        string userId) => builder.Set(x => x.InitialUser = userId);


    // External Select menu base
    public static InputElementBuilder<TElement> MinQueryLength<TElement>(
        this InputElementBuilder<TElement> builder,
        int minQueryLength)
        where TElement : ExternalSelectMenuBase
        => builder.Set(x => x.MinQueryLength = minQueryLength);

    // External Select Menu
    public static InputElementBuilder<ExternalSelectMenu> InitialOption(
        this InputElementBuilder<ExternalSelectMenu> builder,
        Option option) => builder.Set(x => x.InitialOption = option);

    // Conversation Select Menu
    public static InputElementBuilder<ConversationSelectMenu> InitialConversation(
        this InputElementBuilder<ConversationSelectMenu> builder,
        string conversationId) => builder.Set(x => x.InitialConversation = conversationId);

    public static InputElementBuilder<ConversationSelectMenu> DefaultToCurrentConversation(
        this InputElementBuilder<ConversationSelectMenu> builder,
        bool defaultToCurrentConversation = true) =>
        builder.Set(x => x.DefaultToCurrentConversation = defaultToCurrentConversation);

    public static InputElementBuilder<ConversationSelectMenu> Filter(
        this InputElementBuilder<ConversationSelectMenu> builder,
        Action<ConversationFilter> createFilter)
    {
        var filter = builder.Element.Filter ?? new ConversationFilter();
        createFilter(filter);
        return builder.Set(x => x.Filter = filter);
    }

    public static InputElementBuilder<ConversationSelectMenu> Include(
        this InputElementBuilder<ConversationSelectMenu> builder,
        ConversationTypeFilter type) =>
        builder.Filter(x => x.Include.Add(type));

    public static InputElementBuilder<ConversationSelectMenu> ExcludeExternalSharedChannels(
        this InputElementBuilder<ConversationSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeExternalSharedChannels = exclude);

    public static InputElementBuilder<ConversationSelectMenu> ExcludeBotUsers(
        this InputElementBuilder<ConversationSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeBotUsers = exclude);

    // Channel Select Menu
    public static InputElementBuilder<ChannelSelectMenu> InitialChannel(
        this InputElementBuilder<ChannelSelectMenu> builder,
        string channelId) => builder.Set(x => x.InitialChannel = channelId);

    // Static Multi Select Menu
    public static InputElementBuilder<TElement> InitialOptions<TElement>(
        this InputElementBuilder<TElement> builder,
        Func<IList<Option>, IList<Option>> selector) where TElement : StaticMultiSelectMenu =>
        builder.Set(x => x.InitialOptions = selector(x.Options));

    public static InputElementBuilder<TElement> InitialOptions<TElement>(
        this InputElementBuilder<TElement> builder,
        params string[] initialOptions) where TElement : StaticMultiSelectMenu =>
        builder.InitialOptions(options => options.Where(o => initialOptions.Contains(o.Value)).ToList());

    public static InputElementBuilder<TElement> MaxSelectedItems<TElement>(
        this InputElementBuilder<TElement> builder,
        int maxItems) where TElement : StaticMultiSelectMenu =>
        builder.Set(x => x.MaxSelectedItems = maxItems);


    // external multi select menu
    public static InputElementBuilder<ExternalMultiSelectMenu> InitialOptions(
        this InputElementBuilder<ExternalMultiSelectMenu> builder,
        IList<Option> initialOptions) => 
        builder.Set(x => x.InitialOptions = initialOptions);

    // user multi select menu

    public static InputElementBuilder<UserMultiSelectMenu> InitialUsers(
        this InputElementBuilder<UserMultiSelectMenu> builder,
        params string[] userIds) => builder.Set(x => x.InitialUsers = userIds);

    public static InputElementBuilder<UserMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<UserMultiSelectMenu> builder,
        int maxItems) => builder.Set(x => x.MaxSelectedItems = maxItems);

    // conversation multi select menu

    public static InputElementBuilder<ConversationMultiSelectMenu> InitialConversations(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        params string[] conversationIds) => builder.Set(x => x.InitialConversations = conversationIds);

    public static InputElementBuilder<ConversationMultiSelectMenu> DefaultToCurrentConversation(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        bool defaultToCurrentConversation = true) =>
        builder.Set(x => x.DefaultToCurrentConversation = defaultToCurrentConversation);

    public static InputElementBuilder<ConversationMultiSelectMenu> Filter(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        Action<ConversationFilter> createFilter)
    {
        var filter = builder.Element.Filter ?? new ConversationFilter();
        createFilter(filter);
        return builder.Set(x => x.Filter = filter);
    }

    public static InputElementBuilder<ConversationMultiSelectMenu> Include(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        ConversationTypeFilter type) =>
        builder.Filter(x => x.Include.Add(type));

    public static InputElementBuilder<ConversationMultiSelectMenu> ExcludeExternalSharedChannels(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeExternalSharedChannels = exclude);

    public static InputElementBuilder<ConversationMultiSelectMenu> ExcludeBotUsers(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        bool exclude = true) => builder.Filter(x => x.ExcludeBotUsers = exclude);

    public static InputElementBuilder<ConversationMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<ConversationMultiSelectMenu> builder,
        int maxItems) => builder.Set(x => x.MaxSelectedItems = maxItems);

    // channel multi select menu
    public static InputElementBuilder<ChannelMultiSelectMenu> InitialChannels(
        this InputElementBuilder<ChannelMultiSelectMenu> builder,
        params string[] channelIds) => builder.Set(x => x.InitialChannels = channelIds);

    public static InputElementBuilder<ChannelMultiSelectMenu> MaxSelectedItems(
        this InputElementBuilder<ChannelMultiSelectMenu> builder,
        int maxItems) => builder.Set(x => x.MaxSelectedItems = maxItems);
}