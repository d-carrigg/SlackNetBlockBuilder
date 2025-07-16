using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(ButtonExtensions))]
public class ButtonExtensionsTest
{
    [Fact]
    public void Text_SetsButtonText()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.Text("Click me");
        
        // Assert
        Assert.Equal("Click me", button.Text.Text);
        Assert.Same(builder, result); // Ensures method returns the same builder for chaining
    }
    
    [Fact]
    public void Url_SetsButtonUrl()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.Url("https://example.com");
        
        // Assert
        Assert.Equal("https://example.com", button.Url);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Value_SetsButtonValue()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.Value("button_value");
        
        // Assert
        Assert.Equal("button_value", button.Value);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Style_SetsButtonStyle()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.Style(ButtonStyle.Primary);
        
        // Assert
        Assert.Equal(ButtonStyle.Primary, button.Style);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void AccessibilityLabel_SetsButtonAccessibilityLabel()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        var result = builder.AccessibilityLabel("Accessible button");
        
        // Assert
        Assert.Equal("Accessible button", button.AccessibilityLabel);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void AddButton_WithStyle_AddsButtonToActionsBlock()
    {
        // Arrange
        var actionsBuilder = ActionsBlockBuilder.Create();
        
        // Act
        var result = actionsBuilder.AddButton(
            "action_id", 
            "Click me", 
            ButtonStyle.Danger, 
            "https://example.com", 
            "button_value");
        
        var block = actionsBuilder.Build();
        
        // Assert
        Assert.Same(actionsBuilder, result);
        Assert.Single(block.Elements);
        
        var button = Assert.IsType<Button>(block.Elements[0]);
        Assert.Equal("action_id", button.ActionId);
        Assert.Equal("Click me", button.Text.Text);
        Assert.Equal(ButtonStyle.Danger, button.Style);
        Assert.Equal("https://example.com", button.Url);
        Assert.Equal("button_value", button.Value);
    }
    
    [Fact]
    public void AddButton_WithoutStyle_AddsButtonWithDefaultStyle()
    {
        // Arrange
        var actionsBuilder = ActionsBlockBuilder.Create();
        
        // Act
        var result = actionsBuilder.AddButton(
            "action_id", 
            "Click me", 
            "https://example.com", 
            "button_value");
        
        var block = actionsBuilder.Build();
        
        // Assert
        Assert.Same(actionsBuilder, result);
        Assert.Single(block.Elements);
        
        var button = Assert.IsType<Button>(block.Elements[0]);
        Assert.Equal("action_id", button.ActionId);
        Assert.Equal("Click me", button.Text.Text);
        Assert.Equal(ButtonStyle.Default, button.Style);
        Assert.Equal("https://example.com", button.Url);
        Assert.Equal("button_value", button.Value);
    }
    
    [Fact]
    public void AddButton_WithNullOptionalParameters_SetsNullValues()
    {
        // Arrange
        var actionsBuilder = ActionsBlockBuilder.Create();
        
        // Act
        actionsBuilder.AddButton("action_id", "Click me");
        var block = actionsBuilder.Build();
        
        // Assert
        var button = Assert.IsType<Button>(block.Elements[0]);
        Assert.Equal("action_id", button.ActionId);
        Assert.Equal("Click me", button.Text.Text);
        Assert.Equal(ButtonStyle.Default, button.Style);
        Assert.Null(button.Url);
        Assert.Null(button.Value);
    }
    
    [Fact]
    public void ChainedMethods_BuildCorrectButton()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act
        builder
            .Text("Click me")
            .Url("https://example.com")
            .Value("button_value")
            .Style(ButtonStyle.Primary)
            .AccessibilityLabel("Accessible button");
        
        // Assert
        Assert.Equal("Click me", button.Text.Text);
        Assert.Equal("https://example.com", button.Url);
        Assert.Equal("button_value", button.Value);
        Assert.Equal(ButtonStyle.Primary, button.Style);
        Assert.Equal("Accessible button", button.AccessibilityLabel);
    }
    
    [Fact]
    public void Url_WithUri_SetsButtonUrl()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        var uri = new Uri("https://example.com");
        
        // Act
        var result = builder.Url(uri);
        
        // Assert
        Assert.Equal("https://example.com", button.Url);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void Url_WithNullUri_ThrowsArgumentNullException()
    {
        // Arrange
        var button = new Button();
        var builder = new ActionElementBuilder<Button>(button);
        
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => builder.Url((Uri)null));
    }
    
    [Fact]
    public void AddButton_WithUriParameter_AddsButtonToActionsBlock()
    {
        // Arrange
        var actionsBuilder = ActionsBlockBuilder.Create();
        var uri = new Uri("https://example.com");
        
        // Act
        var result = actionsBuilder.AddButton(
            "action_id", 
            "Click me", 
            ButtonStyle.Primary, 
            uri, 
            "button_value");
        
        var block = actionsBuilder.Build();
        
        // Assert
        Assert.Same(actionsBuilder, result);
        Assert.Single(block.Elements);
        var button = Assert.IsType<Button>(block.Elements[0]);
        Assert.Equal("action_id", button.ActionId);
        Assert.Equal("Click me", button.Text.Text);
        Assert.Equal(ButtonStyle.Primary, button.Style);
        Assert.Equal("https://example.com", button.Url);
        Assert.Equal("button_value", button.Value);
    }
    
    [Fact]
    public void AddButton_WithNullUri_AddsButtonWithNullUrl()
    {
        // Arrange
        var actionsBuilder = ActionsBlockBuilder.Create();
        
        // Act
        var result = actionsBuilder.AddButton(
            "action_id", 
            "Click me", 
            ButtonStyle.Primary, 
            (Uri)null, 
            "button_value");
        
        var block = actionsBuilder.Build();
        
        // Assert
        Assert.Same(actionsBuilder, result);
        Assert.Single(block.Elements);
        var button = Assert.IsType<Button>(block.Elements[0]);
        Assert.Equal("action_id", button.ActionId);
        Assert.Equal("Click me", button.Text.Text);
        Assert.Equal(ButtonStyle.Primary, button.Style);
        Assert.Null(button.Url);
        Assert.Equal("button_value", button.Value);
    }
}
