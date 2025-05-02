using JetBrains.Annotations;
using SlackNet.Blocks;
using System.Diagnostics;

namespace UnitTests;

[TestSubject(typeof(SelectMenuBaseExtensions))]
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
                builder.AddOptionGroup($"Group {j}", group =>
                {
                    for (int k = 0; k < 5; k++)
                    {
                        group.AddOption($"group_{j}_value_{k}", $"Group {j} Option {k}");
                    }
                });
            }
        }
        
        stopwatch.Stop();
        
        // Output performance metrics
        var msPerOperation = stopwatch.ElapsedMilliseconds / (double)iterations;
        Console.WriteLine($"SelectMenuBaseExtensions operations took {msPerOperation:F6} ms per iteration");
        
        // No specific assertion, this is a baseline measurement
    }
} 