using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(RadioButtonGroupExtensions))]
public class RadioButtonGroupExtensionsTest
{
    [Fact]
    public void AddOption_AddsOptionToRadioButtonGroup()
    {
        // Arrange
        var radioButtonGroup = new RadioButtonGroup();
        var builder = new InputElementBuilder<RadioButtonGroup>(radioButtonGroup);
        
        // Act
        var result = builder.AddOption("value1", "Option 1");
        
        // Assert
        Assert.Single(radioButtonGroup.Options);
        Assert.Equal("value1", radioButtonGroup.Options[0].Value);
        Assert.Equal("Option 1", radioButtonGroup.Options[0].Text.Text);
        Assert.Null(radioButtonGroup.Options[0].Description);
        Assert.Same(builder, result); // Ensures method returns the same builder for chaining
    }
    
    [Fact]
    public void AddOption_WithDescription_AddsOptionWithDescription()
    {
        // Arrange
        var radioButtonGroup = new RadioButtonGroup();
        var builder = new InputElementBuilder<RadioButtonGroup>(radioButtonGroup);
        var description = new PlainText { Text = "Description text" };
        
        // Act
        var result = builder.AddOption("value1", "Option 1", description);
        
        // Assert
        Assert.Single(radioButtonGroup.Options);
        Assert.Equal("value1", radioButtonGroup.Options[0].Value);
        Assert.Equal("Option 1", radioButtonGroup.Options[0].Text.Text);
        Assert.Same(description, radioButtonGroup.Options[0].Description);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void InitialOption_SetsInitialOptionByValue()
    {
        // Arrange
        var radioButtonGroup = new RadioButtonGroup();
        var builder = new InputElementBuilder<RadioButtonGroup>(radioButtonGroup);
        
        // Add some options
        builder.AddOption("value1", "Option 1");
        builder.AddOption("value2", "Option 2");
        builder.AddOption("value3", "Option 3");
        
        // Act
        var result = builder.InitialOption("value2");
        
        // Assert
        Assert.NotNull(radioButtonGroup.InitialOption);
        Assert.Equal("value2", radioButtonGroup.InitialOption.Value);
        Assert.Equal("Option 2", radioButtonGroup.InitialOption.Text.Text);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void InitialOption_WithNonExistentValue_SetsInitialOptionToNull()
    {
        // Arrange
        var radioButtonGroup = new RadioButtonGroup();
        var builder = new InputElementBuilder<RadioButtonGroup>(radioButtonGroup);
        
        // Add some options
        builder.AddOption("value1", "Option 1");
        builder.AddOption("value2", "Option 2");
        
        // Set an initial option first
        builder.InitialOption("value1");
        Assert.NotNull(radioButtonGroup.InitialOption);
        
        // Act
        var result = builder.InitialOption("value3");
        
        // Assert
        Assert.Null(radioButtonGroup.InitialOption);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void FocusOnLoad_SetsFocusOnLoadProperty()
    {
        // Arrange
        var radioButtonGroup = new RadioButtonGroup();
        var builder = new InputElementBuilder<RadioButtonGroup>(radioButtonGroup);
        
        // Act
        var result = builder.FocusOnLoad();
        
        // Assert
        Assert.True(radioButtonGroup.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void FocusOnLoad_WithFalseParameter_SetsFocusOnLoadToFalse()
    {
        // Arrange
        var radioButtonGroup = new RadioButtonGroup { FocusOnLoad = true };
        var builder = new InputElementBuilder<RadioButtonGroup>(radioButtonGroup);
        
        // Act
        var result = builder.FocusOnLoad(false);
        
        // Assert
        Assert.False(radioButtonGroup.FocusOnLoad);
        Assert.Same(builder, result);
    }
    
    [Fact]
    public void ChainedMethods_BuildCorrectRadioButtonGroup()
    {
        // Arrange
        var radioButtonGroup = new RadioButtonGroup();
        var builder = new InputElementBuilder<RadioButtonGroup>(radioButtonGroup);
        
        // Act
        builder
            .AddOption("value1", "Option 1")
            .AddOption("value2", "Option 2")
            .FocusOnLoad()
            .InitialOption("value2");
        
        // Assert
        Assert.Equal(2, radioButtonGroup.Options.Count);
        Assert.True(radioButtonGroup.FocusOnLoad);
        Assert.NotNull(radioButtonGroup.InitialOption);
        Assert.Equal("value2", radioButtonGroup.InitialOption.Value);
    }
    
    [Fact]
    public void AddRadioButtonGroup_InInputBlock_AddsRadioButtonGroupToBlock()
    {
        // Arrange
        var blockBuilder = BlockBuilder.Create();
        
        // Act
        blockBuilder.AddInput<RadioButtonGroup>("label_text", input => 
        {
            input.Element.ActionId = "radio_1";
            input
                .AddOption("value1", "Option 1")
                .AddOption("value2", "Option 2")
                .InitialOption("value1")
                .FocusOnLoad();
        });
        
        var blocks = blockBuilder.Build();
        
        // Assert
        Assert.Single(blocks);
        var inputBlock = Assert.IsType<InputBlock>(blocks[0]);
        Assert.Equal("label_text", inputBlock.Label.Text);
        
        var radioButtonGroup = Assert.IsType<RadioButtonGroup>(inputBlock.Element);
        Assert.Equal("radio_1", radioButtonGroup.ActionId);
        Assert.Equal(2, radioButtonGroup.Options.Count);
        Assert.Equal("value1", radioButtonGroup.InitialOption.Value);
        Assert.True(radioButtonGroup.FocusOnLoad);
    }
} 