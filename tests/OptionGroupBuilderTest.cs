using JetBrains.Annotations;
using SlackNet.Blocks;

namespace UnitTests;

[TestSubject(typeof(OptionGroupBuilder))]
public class OptionGroupBuilderTest
{
    [Fact]
    public void Constructor_WithValidElement_InitializesCorrectly()
    {
        // Arrange
        var optionGroup = new OptionGroup { Label = new PlainText { Text = "Test Group" } };
        
        // Act
        var builder = new OptionGroupBuilder(optionGroup);
        
        // Assert
        Assert.Same(optionGroup, builder.Element);
    }

    [Fact]
    public void AddOption_WithValueAndText_AddsOptionCorrectly()
    {
        // Arrange
        var optionGroup = new OptionGroup();
        var builder = new OptionGroupBuilder(optionGroup);
        
        // Act
        var result = builder.AddOption("value1", "Option 1");
        
        // Assert
        Assert.Same(builder, result);
        Assert.NotNull(builder.Element.Options);
        Assert.Single(builder.Element.Options);
        
        var option = builder.Element.Options[0];
        Assert.Equal("value1", option.Value);
        Assert.Equal("Option 1", option.Text.Text);
        Assert.Null(option.Description);
    }

    [Fact]
    public void AddOption_WithValueTextAndDescription_AddsOptionCorrectly()
    {
        // Arrange
        var optionGroup = new OptionGroup();
        var builder = new OptionGroupBuilder(optionGroup);
        var description = new PlainText { Text = "Option description" };
        
        // Act
        var result = builder.AddOption("value1", "Option 1", description);
        
        // Assert
        Assert.Same(builder, result);
        Assert.NotNull(builder.Element.Options);
        Assert.Single(builder.Element.Options);
        
        var option = builder.Element.Options[0];
        Assert.Equal("value1", option.Value);
        Assert.Equal("Option 1", option.Text.Text);
        Assert.Same(description, option.Description);
    }

    [Fact]
    public void AddOption_WithNullDescription_AddsOptionWithNullDescription()
    {
        // Arrange
        var optionGroup = new OptionGroup();
        var builder = new OptionGroupBuilder(optionGroup);
        
        // Act
        var result = builder.AddOption("value1", "Option 1", null);
        
        // Assert
        Assert.Same(builder, result);
        Assert.NotNull(builder.Element.Options);
        Assert.Single(builder.Element.Options);
        
        var option = builder.Element.Options[0];
        Assert.Equal("value1", option.Value);
        Assert.Equal("Option 1", option.Text.Text);
        Assert.Null(option.Description);
    }

    // This test covers the missing AddOption(Option) method (0% coverage)
    [Fact]
    public void AddOption_WithPreConfiguredOption_AddsOptionCorrectly()
    {
        // Arrange
        var optionGroup = new OptionGroup();
        var builder = new OptionGroupBuilder(optionGroup);
        var option = new Option 
        { 
            Value = "custom_value", 
            Text = "Custom Option",
            Description = new PlainText { Text = "Custom description" }
        };
        
        // Act
        var result = builder.AddOption(option);
        
        // Assert
        Assert.Same(builder, result);
        Assert.NotNull(builder.Element.Options);
        Assert.Single(builder.Element.Options);
        Assert.Same(option, builder.Element.Options[0]);
    }

    [Fact]
    public void AddOption_WithExistingOptions_AppendsToList()
    {
        // Arrange
        var optionGroup = new OptionGroup
        {
            Options = new List<Option>
            {
                new Option { Value = "existing", Text = "Existing Option" }
            }
        };
        var builder = new OptionGroupBuilder(optionGroup);
        
        // Act
        builder.AddOption("new_value", "New Option");
        
        // Assert
        Assert.Equal(2, builder.Element.Options.Count);
        Assert.Equal("existing", builder.Element.Options[0].Value);
        Assert.Equal("new_value", builder.Element.Options[1].Value);
    }

    [Fact]
    public void AddOption_WithNullOptionsInitially_InitializesOptionsList()
    {
        // Arrange
        var optionGroup = new OptionGroup { Options = null };
        var builder = new OptionGroupBuilder(optionGroup);
        
        // Act
        builder.AddOption("value1", "Option 1");
        
        // Assert
        Assert.NotNull(builder.Element.Options);
        Assert.Single(builder.Element.Options);
        Assert.Equal("value1", builder.Element.Options[0].Value);
    }

    [Fact]
    public void AddOption_MultipleOptions_AddsAllOptionsCorrectly()
    {
        // Arrange
        var optionGroup = new OptionGroup();
        var builder = new OptionGroupBuilder(optionGroup);
        
        // Act
        builder
            .AddOption("value1", "Option 1")
            .AddOption("value2", "Option 2", new PlainText { Text = "Description 2" })
            .AddOption(new Option { Value = "value3", Text = "Option 3" });
        
        // Assert
        Assert.Equal(3, builder.Element.Options.Count);
        
        Assert.Equal("value1", builder.Element.Options[0].Value);
        Assert.Equal("Option 1", builder.Element.Options[0].Text.Text);
        Assert.Null(builder.Element.Options[0].Description);
        
        Assert.Equal("value2", builder.Element.Options[1].Value);
        Assert.Equal("Option 2", builder.Element.Options[1].Text.Text);
        Assert.NotNull(builder.Element.Options[1].Description);
        
        Assert.Equal("value3", builder.Element.Options[2].Value);
        Assert.Equal("Option 3", builder.Element.Options[2].Text.Text);
    }
}
