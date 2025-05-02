using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(OverflowMenuExtensions))]
public class OverflowMenuExtensionsTest
{
    [Fact]
    public void AddOption_AddsOptionToOverflowMenu()
    {
        // Arrange
        var overflowMenu = new OverflowMenu();
        var builder = new ActionElementBuilder<OverflowMenu>(overflowMenu);
        
        // Act
        var result = builder.AddOption("value1", "Option 1");
        
        // Assert
        Assert.Single(overflowMenu.Options);
        Assert.Equal("value1", overflowMenu.Options[0].Value);
        Assert.Equal("Option 1", overflowMenu.Options[0].Text.Text);
        Assert.Null(overflowMenu.Options[0].Description);
        Assert.Null(overflowMenu.Options[0].Url);
        Assert.Same(builder, result); // Ensures method returns the same builder for chaining
    }
    
    [Fact]
    public void AddOption_WithDescription_AddsOptionWithDescription()
    {
        // Arrange
        var overflowMenu = new OverflowMenu();
        var builder = new ActionElementBuilder<OverflowMenu>(overflowMenu);
        var description = new PlainText { Text = "Description text" };
        
        // Act
        var result = builder.AddOption("value1", "Option 1", description);
        
        // Assert
        Assert.Single(overflowMenu.Options);
        Assert.Equal("value1", overflowMenu.Options[0].Value);
        Assert.Equal("Option 1", overflowMenu.Options[0].Text.Text);
        Assert.Same(description, overflowMenu.Options[0].Description);
        Assert.Null(overflowMenu.Options[0].Url);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void AddOption_WithUrl_AddsOptionWithUrl()
    {
        // Arrange
        var overflowMenu = new OverflowMenu();
        var builder = new ActionElementBuilder<OverflowMenu>(overflowMenu);
        
        // Act
        var result = builder.AddOption("value1", "Option 1", url: "https://example.com");
        
        // Assert
        Assert.Single(overflowMenu.Options);
        Assert.Equal("value1", overflowMenu.Options[0].Value);
        Assert.Equal("Option 1", overflowMenu.Options[0].Text.Text);
        Assert.Null(overflowMenu.Options[0].Description);
        Assert.Equal("https://example.com", overflowMenu.Options[0].Url);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void AddOption_WithDescriptionAndUrl_AddsOptionWithDescriptionAndUrl()
    {
        // Arrange
        var overflowMenu = new OverflowMenu();
        var builder = new ActionElementBuilder<OverflowMenu>(overflowMenu);
        var description = new PlainText { Text = "Description text" };
        
        // Act
        var result = builder.AddOption("value1", "Option 1", description, "https://example.com");
        
        // Assert
        Assert.Single(overflowMenu.Options);
        Assert.Equal("value1", overflowMenu.Options[0].Value);
        Assert.Equal("Option 1", overflowMenu.Options[0].Text.Text);
        Assert.Same(description, overflowMenu.Options[0].Description);
        Assert.Equal("https://example.com", overflowMenu.Options[0].Url);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void AddMultipleOptions_AddsAllOptionsToOverflowMenu()
    {
        // Arrange
        var overflowMenu = new OverflowMenu();
        var builder = new ActionElementBuilder<OverflowMenu>(overflowMenu);
        
        // Act
        builder
            .AddOption("value1", "Option 1")
            .AddOption("value2", "Option 2")
            .AddOption("value3", "Option 3");
        
        // Assert
        Assert.Equal(3, overflowMenu.Options.Count);
        Assert.Equal("value1", overflowMenu.Options[0].Value);
        Assert.Equal("Option 1", overflowMenu.Options[0].Text.Text);
        Assert.Equal("value2", overflowMenu.Options[1].Value);
        Assert.Equal("Option 2", overflowMenu.Options[1].Text.Text);
        Assert.Equal("value3", overflowMenu.Options[2].Value);
        Assert.Equal("Option 3", overflowMenu.Options[2].Text.Text);
    }
    
    [Fact]
    public void AddOption_InActionsBlock_AddsOverflowMenuToBlock()
    {
        // Arrange
        var actionsBuilder = ActionsBlockBuilder.Create();
        
        // Act
        actionsBuilder.AddOverflowMenu("overflow_1", menu => 
            menu.AddOption("value1", "Option 1")
                .AddOption("value2", "Option 2"));
        
        var block = actionsBuilder.Build();
        
        // Assert
        Assert.Single(block.Elements);
        var overflowMenu = Assert.IsType<OverflowMenu>(block.Elements[0]);
        Assert.Equal("overflow_1", overflowMenu.ActionId);
        Assert.Equal(2, overflowMenu.Options.Count);
        Assert.Equal("value1", overflowMenu.Options[0].Value);
        Assert.Equal("Option 1", overflowMenu.Options[0].Text.Text);
        Assert.Equal("value2", overflowMenu.Options[1].Value);
        Assert.Equal("Option 2", overflowMenu.Options[1].Text.Text);
    }
} 