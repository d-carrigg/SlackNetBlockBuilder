using SlackNet.Blocks;

namespace UnitTests;

public class SelectMenuBaseExtensionsTest
{
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
} 