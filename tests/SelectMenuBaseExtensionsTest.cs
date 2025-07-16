using JetBrains.Annotations;
using SlackNet.Blocks;
using System.Diagnostics;
using Xunit.Abstractions;

namespace UnitTests;

[TestSubject(typeof(SelectMenuBaseExtensions))]
public class SelectMenuBaseExtensionsTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SelectMenuBaseExtensionsTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void StaticSelect_WithInitialOption_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<StaticSelectMenu>(new StaticSelectMenu());

        // Act
        builder
            .AddOption("value1", "Option 1")
            .AddOption("value2", "Option 2")
            .InitialOption("value1");

        // Assert
        Assert.NotNull(builder.Element.InitialOption);
        Assert.Equal("value1", builder.Element.InitialOption.Value);
        Assert.Equal("Option 1", builder.Element.InitialOption.Text.Text);
    }

    [Fact]
    public void ExternalSelect_WithInitialOption_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ExternalSelectMenu>(new ExternalSelectMenu());


        // Act
        var opt = new Option()
        {
            Text = "Option 1",
            Value = "value1"
        };
        builder.InitialOption(opt);

        // Assert
        Assert.Equal(opt, builder.Element.InitialOption);
    }

    [Fact]
    public void ConversationSelect_WithFilter_AppliesCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ConversationSelectMenu>(new ConversationSelectMenu());

        // Act
        builder
            .Include(ConversationTypeFilter.Im)
            .ExcludeBotUsers()
            .ExcludeExternalSharedChannels();

        // Assert
        Assert.NotNull(builder.Element.Filter);
        Assert.Contains(ConversationTypeFilter.Im, builder.Element.Filter.Include);
        Assert.True(builder.Element.Filter.ExcludeBotUsers);
        Assert.True(builder.Element.Filter.ExcludeExternalSharedChannels);
    }

    [Fact]
    public void MultiSelect_WithMaxItems_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<StaticMultiSelectMenu>(new StaticMultiSelectMenu());

        // Act
        builder.MaxSelectedItems(5);

        // Assert
        Assert.Equal(5, builder.Element.MaxSelectedItems);
    }

    [Fact]
    public void Placeholder_SetsPlaceholderText()
    {
        // Arrange
        var menu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(menu);

        // Act
        var result = builder.Placeholder("Select an option");

        // Assert
        Assert.Equal("Select an option", menu.Placeholder.Text);
        Assert.Same(builder, result);
    }

    [Fact]
    public void FocusOnLoad_SetsFocusProperty()
    {
        // Arrange
        var menu = new StaticSelectMenu();
        var builder = new InputElementBuilder<StaticSelectMenu>(menu);

        // Act
        var result = builder.FocusOnLoad();

        // Assert
        Assert.True(menu.FocusOnLoad);
        Assert.Same(builder, result);
    }

    [Fact]
    public void Performance_AddingManyOptions()
    {
        // Arrange
        const int iterations = 1000;
        var stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < iterations; i++)
        {
            var menu = new StaticSelectMenu();
            var builder = new InputElementBuilder<StaticSelectMenu>(menu);

            // Add multiple options
            for (int j = 0; j < 20; j++)
            {
                builder.AddOption($"value_{j}", $"Option {j} in iteration {i}");
            }

            // Add option groups
            for (int j = 0; j < 5; j++)
            {
                var j1 = j;
                builder.AddOptionGroup($"Group {j1}", group =>
                {
                    for (int k = 0; k < 5; k++)
                    {
                        var k1 = k;
                        group.AddOption($"group_{j1}_value_{k1}", $"Group {j1} Option {k1}");
                    }
                });
            }
        }

        stopwatch.Stop();

        // Output performance metrics
        var msPerOperation = stopwatch.ElapsedMilliseconds / (double)iterations;
        _testOutputHelper.WriteLine($"SelectMenuBaseExtensions operations took {msPerOperation:F6} ms per iteration");

        // No specific assertion, this is a baseline measurement
    }

    // --- StaticMultiSelectMenu Tests ---

    [Fact]
    public void StaticMultiSelect_InitialOptionsByValue_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<StaticMultiSelectMenu>(new StaticMultiSelectMenu());
        builder
            .AddOption("val1", "Opt1")
            .AddOption("val2", "Opt2")
            .AddOption("val3", "Opt3");

        // Act
        builder.InitialOptions("val1", "val3");

        // Assert
        Assert.NotNull(builder.Element.InitialOptions);
        Assert.Equal(2, builder.Element.InitialOptions.Count);
        Assert.Contains(builder.Element.Options[0], builder.Element.InitialOptions);
        Assert.Contains(builder.Element.Options[2], builder.Element.InitialOptions);
    }



    [Fact]
    public void StaticMultiSelect_InitialOptionsByValue_DoesNotSetInvalidOption()
    {
        // Arrange
        var builder = new InputElementBuilder<StaticMultiSelectMenu>(new StaticMultiSelectMenu());
        builder
            .AddOption("val1", "Opt1")
            .AddOption("val2", "Opt2")
            .AddOption("val3", "Opt3");


        // Act
        builder.InitialOptions("val1", "invalid_val");

        // Assert
        Assert.NotNull(builder.Element.InitialOptions);
        Assert.Equal(1, builder.Element.InitialOptions.Count);
    }



    [Fact]
    public void StaticMultiSelect_InitialOptionsBySelector_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<StaticMultiSelectMenu>(new StaticMultiSelectMenu());
        builder
            .AddOption("val1", "Opt1")
            .AddOption("val2", "Opt2")
            .AddOption("val3", "Opt3");

        // Act
        builder.InitialOptions(options => options.Where(o => o.Value != "val2").ToList());

        // Assert
        Assert.NotNull(builder.Element.InitialOptions);
        Assert.Equal(2, builder.Element.InitialOptions.Count);
        Assert.Contains(builder.Element.Options[0], builder.Element.InitialOptions);
        Assert.Contains(builder.Element.Options[2], builder.Element.InitialOptions);
    }

    // --- UserSelectMenu Tests ---

    [Fact]
    public void UserSelect_InitialUser_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<UserSelectMenu>(new UserSelectMenu());
        const string userId = "U12345";

        // Act
        builder.InitialUser(userId);

        // Assert
        Assert.Equal(userId, builder.Element.InitialUser);
    }

    // --- UserMultiSelectMenu Tests ---

    [Fact]
    public void UserMultiSelect_InitialUsers_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<UserMultiSelectMenu>(new UserMultiSelectMenu());
        const string user1 = "U123";
        const string user2 = "U456";

        // Act
        builder.InitialUsers(user1, user2);

        // Assert
        Assert.NotNull(builder.Element.InitialUsers);
        Assert.Equal(new[] { user1, user2 }, builder.Element.InitialUsers);
    }

    [Fact]
    public void UserMultiSelect_MaxSelectedItems_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<UserMultiSelectMenu>(new UserMultiSelectMenu());

        // Act
        builder.MaxSelectedItems(3);

        // Assert
        Assert.Equal(3, builder.Element.MaxSelectedItems);
    }

    // --- ExternalSelectMenu Tests ---

    [Fact]
    public void ExternalSelect_MinQueryLength_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ExternalSelectMenu>(new ExternalSelectMenu());

        // Act
        builder.MinQueryLength(2);

        // Assert
        Assert.Equal(2, builder.Element.MinQueryLength);
    }

    // --- ExternalMultiSelectMenu Tests ---

    [Fact]
    public void ExternalMultiSelect_MinQueryLength_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ExternalMultiSelectMenu>(new ExternalMultiSelectMenu());

        // Act
        builder.MinQueryLength(3);

        // Assert
        Assert.Equal(3, builder.Element.MinQueryLength);
    }

    // --- ConversationSelectMenu Tests ---

    [Fact]
    public void ConversationSelect_InitialConversation_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ConversationSelectMenu>(new ConversationSelectMenu());
        const string convId = "C12345";

        // Act
        builder.InitialConversation(convId);

        // Assert
        Assert.Equal(convId, builder.Element.InitialConversation);
    }

    [Fact]
    public void ConversationSelect_DefaultToCurrentConversation_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ConversationSelectMenu>(new ConversationSelectMenu());

        // Act
        builder.DefaultToCurrentConversation();

        // Assert
        Assert.True(builder.Element.DefaultToCurrentConversation);
    }

    // --- ConversationMultiSelectMenu Tests ---

    [Fact]
    public void ConversationMultiSelect_InitialConversations_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ConversationMultiSelectMenu>(new ConversationMultiSelectMenu());
        const string conv1 = "C123";
        const string conv2 = "G456";

        // Act
        builder.InitialConversations(conv1, conv2);

        // Assert
        Assert.NotNull(builder.Element.InitialConversations);
        Assert.Equal(new[] { conv1, conv2 }, builder.Element.InitialConversations);
    }

    [Fact]
    public void ConversationMultiSelect_DefaultToCurrentConversation_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ConversationMultiSelectMenu>(new ConversationMultiSelectMenu());

        // Act
        builder.DefaultToCurrentConversation(false); // Test explicit false

        // Assert
        Assert.False(builder.Element.DefaultToCurrentConversation);

        // Act again (default true)
        builder.DefaultToCurrentConversation();

        // Assert
        Assert.True(builder.Element.DefaultToCurrentConversation);
    }

    [Fact]
    public void ConversationMultiSelect_Filter_AppliesCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ConversationMultiSelectMenu>(new ConversationMultiSelectMenu());

        // Act
        builder.Filter(f =>
        {
            f.Include.Add(ConversationTypeFilter.Private);
            f.ExcludeBotUsers = true;
        });

        // Assert
        Assert.NotNull(builder.Element.Filter);
        Assert.Contains(ConversationTypeFilter.Private, builder.Element.Filter.Include);
        Assert.True(builder.Element.Filter.ExcludeBotUsers);
        Assert.False(builder.Element.Filter.ExcludeExternalSharedChannels); // Check default
    }

    [Fact]
    public void ConversationMultiSelect_Include_AppliesCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ConversationMultiSelectMenu>(new ConversationMultiSelectMenu());

        // Act
        builder.Include(ConversationTypeFilter.Public);

        // Assert
        Assert.NotNull(builder.Element.Filter);
        Assert.Contains(ConversationTypeFilter.Public, builder.Element.Filter.Include);
    }

    [Fact]
    public void ConversationMultiSelect_ExcludeMethods_AppliesCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ConversationMultiSelectMenu>(new ConversationMultiSelectMenu());

        // Act
        builder.ExcludeBotUsers();
        builder.ExcludeExternalSharedChannels();

        // Assert
        Assert.NotNull(builder.Element.Filter);
        Assert.True(builder.Element.Filter.ExcludeBotUsers);
        Assert.True(builder.Element.Filter.ExcludeExternalSharedChannels);
    }

    [Fact]
    public void ConversationMultiSelect_MaxSelectedItems_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ConversationMultiSelectMenu>(new ConversationMultiSelectMenu());

        // Act
        builder.MaxSelectedItems(4);

        // Assert
        Assert.Equal(4, builder.Element.MaxSelectedItems);
    }

    // --- ChannelSelectMenu Tests ---

    [Fact]
    public void ChannelSelect_InitialChannel_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ChannelSelectMenu>(new ChannelSelectMenu());
        const string channelId = "C98765";

        // Act
        builder.InitialChannel(channelId);

        // Assert
        Assert.Equal(channelId, builder.Element.InitialChannel);
    }

    // --- ChannelMultiSelectMenu Tests ---

    [Fact]
    public void ChannelMultiSelect_InitialChannels_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ChannelMultiSelectMenu>(new ChannelMultiSelectMenu());
        const string chan1 = "C123";
        const string chan2 = "C456";

        // Act
        builder.InitialChannels(chan1, chan2);

        // Assert
        Assert.NotNull(builder.Element.InitialChannels);
        Assert.Equal(new[] { chan1, chan2 }, builder.Element.InitialChannels);
    }

    [Fact]
    public void ChannelMultiSelect_MaxSelectedItems_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ChannelMultiSelectMenu>(new ChannelMultiSelectMenu());

        // Act
        builder.MaxSelectedItems(2);

        // Assert
        Assert.Equal(2, builder.Element.MaxSelectedItems);
    }

    [Fact]
    public void StaticMultiSelectMenu_InitialOptions_WithStringArray_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<StaticMultiSelectMenu>(new StaticMultiSelectMenu());

        // Act
        builder
            .AddOption("value1", "Option 1")
            .AddOption("value2", "Option 2")
            .AddOption("value3", "Option 3")
            .InitialOptions("value1", "value3");

        // Assert
        Assert.NotNull(builder.Element.InitialOptions);
        Assert.Equal(2, builder.Element.InitialOptions.Count);
        Assert.Contains(builder.Element.InitialOptions, opt => opt.Value == "value1");
        Assert.Contains(builder.Element.InitialOptions, opt => opt.Value == "value3");
    }

    [Fact]
    public void StaticMultiSelectMenu_InitialOptions_WithEmptyArray_SetsEmptyList()
    {
        // Arrange
        var builder = new InputElementBuilder<StaticMultiSelectMenu>(new StaticMultiSelectMenu());

        // Act
        builder
            .AddOption("value1", "Option 1")
            .AddOption("value2", "Option 2")
            .InitialOptions();

        // Assert
        Assert.NotNull(builder.Element.InitialOptions);
        Assert.Empty(builder.Element.InitialOptions);
    }

    [Fact]
    public void StaticMultiSelectMenu_InitialOptions_WithNonExistentValues_FiltersCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<StaticMultiSelectMenu>(new StaticMultiSelectMenu());

        // Act
        builder
            .AddOption("value1", "Option 1")
            .AddOption("value2", "Option 2")
            .InitialOptions("value1", "nonexistent", "value2");

        // Assert
        Assert.NotNull(builder.Element.InitialOptions);
        Assert.Equal(2, builder.Element.InitialOptions.Count);
        Assert.Contains(builder.Element.InitialOptions, opt => opt.Value == "value1");
        Assert.Contains(builder.Element.InitialOptions, opt => opt.Value == "value2");
    }

    [Fact]
    public void StaticMultiSelectMenu_InitialOptions_WithNullArray_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new InputElementBuilder<StaticMultiSelectMenu>(new StaticMultiSelectMenu());

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.InitialOptions((string[])null));
    }

    [Fact]
    public void ExternalMultiSelectMenu_InitialOptions_WithOptionList_SetsCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ExternalMultiSelectMenu>(new ExternalMultiSelectMenu());
        var options = new List<Option>
        {
            new Option { Text = "Option 1", Value = "value1" },
            new Option { Text = "Option 2", Value = "value2" },
            new Option { Text = "Option 3", Value = "value3" }
        };

        // Act
        builder.InitialOptions(options);

        // Assert
        Assert.NotNull(builder.Element.InitialOptions);
        Assert.Equal(3, builder.Element.InitialOptions.Count);
        Assert.Same(options, builder.Element.InitialOptions);
    }

    [Fact]
    public void ExternalMultiSelectMenu_InitialOptions_WithEmptyList_SetsEmptyList()
    {
        // Arrange
        var builder = new InputElementBuilder<ExternalMultiSelectMenu>(new ExternalMultiSelectMenu());
        var emptyOptions = new List<Option>();

        // Act
        builder.InitialOptions(emptyOptions);

        // Assert
        Assert.NotNull(builder.Element.InitialOptions);
        Assert.Empty(builder.Element.InitialOptions);
        Assert.Same(emptyOptions, builder.Element.InitialOptions);
    }

    [Fact]
    public void ExternalMultiSelectMenu_InitialOptions_WithNullList_ThrowsArgumentNullException()
    {
        // Arrange
        var builder = new InputElementBuilder<ExternalMultiSelectMenu>(new ExternalMultiSelectMenu());

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.InitialOptions((IList<Option>)null!));
    }

    [Fact]
    public void ExternalMultiSelectMenu_InitialOptions_ChainedWithOtherMethods_WorksCorrectly()
    {
        // Arrange
        var builder = new InputElementBuilder<ExternalMultiSelectMenu>(new ExternalMultiSelectMenu());
        var options = new List<Option>
        {
            new Option { Text = "Option 1", Value = "value1" },
            new Option { Text = "Option 2", Value = "value2" }
        };

        // Act
        var result = builder
            .Placeholder("Select options")
            .InitialOptions(options)
            .MaxSelectedItems(5)
            .FocusOnLoad();

        // Assert
        Assert.Same(builder, result);
        Assert.Equal("Select options", builder.Element.Placeholder.Text);
        Assert.Same(options, builder.Element.InitialOptions);
        Assert.Equal(5, builder.Element.MaxSelectedItems);
        Assert.True(builder.Element.FocusOnLoad);
    }
}

