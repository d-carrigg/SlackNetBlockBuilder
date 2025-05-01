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
} 