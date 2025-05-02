using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(CheckboxGroupExtensions))]
public class CheckboxGroupExtensionsTest
{
    [Fact]
    public void AddOption_AddsOptionToCheckboxGroup()
    {
        // Arrange
        var checkboxGroup = new CheckboxGroup();
        var builder = new InputElementBuilder<CheckboxGroup>(checkboxGroup);
        
        // Act
        var result = builder.AddOption("value1", "Option 1");
        
        // Assert
        Assert.Single(checkboxGroup.Options);
        Assert.Equal("value1", checkboxGroup.Options[0].Value);
        Assert.Equal("Option 1", checkboxGroup.Options[0].Text.Text);
        Assert.Null(checkboxGroup.Options[0].Description);
        Assert.Same(builder, result); // Ensures method returns the same builder for chaining
    }
    
    [Fact]
    public void AddOption_WithDescription_AddsOptionWithDescription()
    {
        // Arrange
        var checkboxGroup = new CheckboxGroup();
        var builder = new InputElementBuilder<CheckboxGroup>(checkboxGroup);
        var description = new PlainText { Text = "Description text" };
        
        // Act
        var result = builder.AddOption("value1", "Option 1", description);
        
        // Assert
        Assert.Single(checkboxGroup.Options);
        Assert.Equal("value1", checkboxGroup.Options[0].Value);
        Assert.Equal("Option 1", checkboxGroup.Options[0].Text.Text);
        Assert.Same(description, checkboxGroup.Options[0].Description);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void FocusOnLoad_SetsFocusOnLoadProperty()
    {
        // Arrange
        var checkboxGroup = new CheckboxGroup();
        var builder = new InputElementBuilder<CheckboxGroup>(checkboxGroup);
        
        // Act
        var result = builder.FocusOnLoad();
        
        // Assert
        Assert.True(checkboxGroup.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void FocusOnLoad_WithFalseParameter_SetsFocusOnLoadToFalse()
    {
        // Arrange
        var checkboxGroup = new CheckboxGroup { FocusOnLoad = true };
        var builder = new InputElementBuilder<CheckboxGroup>(checkboxGroup);
        
        // Act
        var result = builder.FocusOnLoad(false);
        
        // Assert
        Assert.False(checkboxGroup.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void InitialOptions_WithSelector_SetsInitialOptions()
    {
        // Arrange
        var checkboxGroup = new CheckboxGroup();
        var builder = new InputElementBuilder<CheckboxGroup>(checkboxGroup);
        
        // Add some options
        builder.AddOption("value1", "Option 1");
        builder.AddOption("value2", "Option 2");
        builder.AddOption("value3", "Option 3");
        
        // Act
        var result = builder.InitialOptions(options => options.Take(2).ToList());
        
        // Assert
        Assert.Equal(2, checkboxGroup.InitialOptions.Count);
        Assert.Equal("value1", checkboxGroup.InitialOptions[0].Value);
        Assert.Equal("value2", checkboxGroup.InitialOptions[1].Value);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void InitialOptions_WithValues_SetsInitialOptionsByValues()
    {
        // Arrange
        var checkboxGroup = new CheckboxGroup();
        var builder = new InputElementBuilder<CheckboxGroup>(checkboxGroup);
        
        // Add some options
        builder.AddOption("value1", "Option 1");
        builder.AddOption("value2", "Option 2");
        builder.AddOption("value3", "Option 3");
        
        // Act
        var result = builder.InitialOptions("value1", "value3");
        
        // Assert
        Assert.Equal(2, checkboxGroup.InitialOptions.Count);
        Assert.Equal("value1", checkboxGroup.InitialOptions[0].Value);
        Assert.Equal("value3", checkboxGroup.InitialOptions[1].Value);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void InitialOptions_WithNonExistentValues_ReturnsEmptyList()
    {
        // Arrange
        var checkboxGroup = new CheckboxGroup();
        var builder = new InputElementBuilder<CheckboxGroup>(checkboxGroup);
        
        // Add some options
        builder.AddOption("value1", "Option 1");
        builder.AddOption("value2", "Option 2");
        
        // Act
        var result = builder.InitialOptions("value3", "value4");
        
        // Assert
        Assert.Empty(checkboxGroup.InitialOptions);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void ChainedMethods_BuildCorrectCheckboxGroup()
    {
        // Arrange
        var checkboxGroup = new CheckboxGroup();
        var builder = new InputElementBuilder<CheckboxGroup>(checkboxGroup);
        
        // Act
        builder
            .AddOption("value1", "Option 1")
            .AddOption("value2", "Option 2")
            .FocusOnLoad()
            .InitialOptions("value2");
        
        // Assert
        Assert.Equal(2, checkboxGroup.Options.Count);
        Assert.True(checkboxGroup.FocusOnLoad);
        Assert.Single(checkboxGroup.InitialOptions);
        Assert.Equal("value2", checkboxGroup.InitialOptions[0].Value);
    }
} 